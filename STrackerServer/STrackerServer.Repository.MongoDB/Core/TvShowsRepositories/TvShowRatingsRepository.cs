// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show ratings repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.TvShowsRepositories
{
    using System;
    using System.Linq;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The television show ratings repository.
    /// </summary>
    public class TvShowRatingsRepository : BaseRatingsRepository<RatingsTvShow, string>, ITvShowRatingsRepository 
    {
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
        public bool AddRating(string id, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id, CollectionPrefix));
            var query = Query<RatingsTvShow>.EQ(ratings => ratings.TvShowId, id);

            var update = Update<RatingsTvShow>.Push(tvr => tvr.Ratings, rating);

            var removeRating = this.Read(id).Ratings.Find(r => r.UserId.Equals(rating.UserId));

            // If already have a rating for the user, need to remove it before insert the new one.
            if (this.RemoveRating(id, removeRating) && this.ModifyList(collection, query, update))
            {
                update = Update<RatingsTvShow>.Set(tvr => tvr.Average, this.Read(id).Ratings.Average(rating1 => rating1.UserRating));
                return collection.Update(query, update).Ok;
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
        public bool RemoveRating(string id, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id, CollectionPrefix));
            var query = Query<RatingsTvShow>.EQ(ratings => ratings.TvShowId, id);
            var update = Update<RatingsTvShow>.Pull(tvr => tvr.Ratings, rating);

            return this.ModifyList(collection, query, update);
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(RatingsTvShow entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.TvShowId, CollectionPrefix));
            this.SetupIndexes(collection); 
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
            var query = Query<RatingsTvShow>.EQ(r => r.TvShowId, id);
            return collection.FindOne<RatingsTvShow>(query, "_id");
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(RatingsTvShow entity)
        {
            throw new NotSupportedException("this method currently is not supported.");
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
            var query = Query<RatingsTvShow>.EQ(ratings => ratings.TvShowId, id);

            collection.FindAndRemove(query, SortBy.Null);
        }
    }
}
