// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episodes ratings repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.EpisodesRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::MongoDB.Bson;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;
    using STrackerServer.Logger.Core;

    /// <summary>
    /// The episode ratings repository.
    /// </summary>
    public class EpisodeRatingsRepository : BaseRatingsRepository<RatingsEpisode, Episode.EpisodeId>, IEpisodeRatingsRepository 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeRatingsRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public EpisodeRatingsRepository(MongoClient client, MongoUrl url, ILogger logger)
            : base(client, url, logger)
        {
        }

        /// <summary>
        /// Add one rating.
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
        public bool AddRating(Episode.EpisodeId id, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvShowId, CollectionPrefix));
            var query = Query<RatingsEpisode>.EQ(r => r.Id, id);
            var update = Update<RatingsEpisode>.Push(er => er.Ratings, rating);

            var ratingDoc = this.Read(id);
            var removeRating = ratingDoc.Ratings.Find(r => r.User.Id.Equals(rating.User.Id));

            // If already have a rating for the user, need to remove it before insert the new one.
            if (this.RemoveRating(id, removeRating) && this.ModifyList(collection, query, update, ratingDoc))
            {
                return this.Update(this.Read(id));
            }

            return false;
        }

        /// <summary>
        /// Remove one rating.
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
        public bool RemoveRating(Episode.EpisodeId id, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvShowId, CollectionPrefix));
            var query = Query<RatingsEpisode>.EQ(r => r.Id, id);
            var update = Update<RatingsEpisode>.Pull(er => er.Ratings, rating);

            return this.ModifyList(collection, query, update, this.Read(id));
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(RatingsEpisode entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.Id.TvShowId, CollectionPrefix));
            collection.Insert(entity);
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="RatingsEpisode"/>.
        /// </returns>
        protected override RatingsEpisode HookRead(Episode.EpisodeId id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvShowId, CollectionPrefix));
            return collection.FindOneByIdAs<RatingsEpisode>(id.ToBsonDocument());
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(RatingsEpisode entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.Id.TvShowId, CollectionPrefix));
            var update = Update<RatingsEpisode>.Set(episode => episode.Average, entity.Ratings.Average(rating1 => rating1.UserRating));
            var query = Query<RatingsEpisode>.EQ(r => r.Id, entity.Id);
            collection.FindAndModify(query, SortBy.Null, update);
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(Episode.EpisodeId id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvShowId, CollectionPrefix));
            var query = Query<RatingsEpisode>.EQ(r => r.Id, id);
            collection.FindAndRemove(query, SortBy.Null);
        }

        /// <summary>
        /// Hook method for Read all operation.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        protected override ICollection<RatingsEpisode> HookReadAll()
        {
            throw new NotSupportedException("this method currently is not supported.");
        }
    }
}