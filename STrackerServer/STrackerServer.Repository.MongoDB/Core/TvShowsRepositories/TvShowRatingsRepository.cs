﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show ratings repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.TvShowsRepositories
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The television show ratings repository.
    /// </summary>
    public class TvShowRatingsRepository : BaseRatingsRepository<RatingsTvShow, string>, ITvShowRatingsRepository 
    {
        /// <summary>
        /// The collection of all television shows synopsis. In this case the 
        /// collection is always the same.
        /// </summary>
        private readonly MongoCollection collectionAll;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowRatingsRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public TvShowRatingsRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
            this.collectionAll = this.Database.GetCollection(ConfigurationManager.AppSettings["AllTvShowsCollection"]);
        }

        /// <summary>
        /// The add rating.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool AddRating(string id, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id, CollectionPrefix));
            var query = Query<RatingsTvShow>.EQ(ratings => ratings.Id, id);
            var update = Update<RatingsTvShow>.Push(tvr => tvr.Ratings, rating);

            var removeRating = this.Read(id).Ratings.Find(r => r.User.Id.Equals(rating.User.Id));

            // If already have a rating for the user, need to remove it before insert the new one.
            if (this.RemoveRating(id, removeRating) && this.ModifyList(collection, query, update, this.Read(id)))
            {
                // Update average rating.
                return this.Update(this.Read(id));
            }

            return false;
        }

        /// <summary>
        /// The remove rating.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool RemoveRating(string id, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id, CollectionPrefix));
            var query = Query<RatingsTvShow>.EQ(ratings => ratings.Id, id);
            var update = Update<RatingsTvShow>.Pull(tvr => tvr.Ratings, rating);

            return this.ModifyList(collection, query, update, this.Read(id));
        }

        /// <summary>
        /// The get top rated.
        /// </summary>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public ICollection<TvShow.TvShowSynopsis> GetTopRated(int max)
        {
            return this.collectionAll.FindAllAs<TvShow.TvShowSynopsis>().OrderByDescending(tvshow => this.Read(tvshow.Id).Average).ToList().Take(max).ToList();
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(RatingsTvShow entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.Id, CollectionPrefix));
            collection.Insert(entity);
        }

        /// <summary>
        /// The hook read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="RatingsTvShow"/>.
        /// </returns>
        protected override RatingsTvShow HookRead(string id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id, CollectionPrefix));
            return collection.FindOneByIdAs<RatingsTvShow>(id);
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(RatingsTvShow entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.Id, CollectionPrefix));
            var update = Update<RatingsTvShow>.Set(tvr => tvr.Average, entity.Ratings.Average(rating => rating.UserRating)).Set(tvr => tvr.Version, entity.Version + 1);
            var query = Query<RatingsTvShow>.EQ(ratings => ratings.Id, entity.Id);

            collection.FindAndModify(query, SortBy.Null, update);
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(string id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id, CollectionPrefix));
            var query = Query<RatingsTvShow>.EQ(ratings => ratings.Id, id);
            
            collection.FindAndRemove(query, SortBy.Null);
        }
    }
}
