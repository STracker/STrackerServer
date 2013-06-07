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

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The episode ratings repository.
    /// </summary>
    public class EpisodeRatingsRepository : BaseRatingsRepository<RatingsEpisode, Tuple<string, int, int>>, IEpisodeRatingsRepository 
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
        public EpisodeRatingsRepository(MongoClient client, MongoUrl url)
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
        public bool AddRating(Tuple<string, int, int> id, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.Item1, CollectionPrefix));
            var query = Query.And(
                Query<RatingsEpisode>.EQ(ratings => ratings.TvShowId, id.Item1),
                Query<RatingsEpisode>.EQ(ratings => ratings.SeasonNumber, id.Item2),
                Query<RatingsEpisode>.EQ(ratings => ratings.EpisodeNumber, id.Item3));

            var update = Update<RatingsEpisode>.Push(er => er.Ratings, rating);

            // If already have a rating for the user, need to remove it before insert the new one.
            return this.RemoveRating(id, rating) && this.ModifyList(collection, query, update);
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
        public bool RemoveRating(Tuple<string, int, int> id, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.Item1, CollectionPrefix));
            var query = Query.And(
                Query<RatingsEpisode>.EQ(ratings => ratings.TvShowId, id.Item1),
                Query<RatingsEpisode>.EQ(ratings => ratings.SeasonNumber, id.Item2),
                Query<RatingsEpisode>.EQ(ratings => ratings.EpisodeNumber, id.Item3));

            var update = Update<RatingsEpisode>.Pull(er => er.Ratings, rating);

            return this.ModifyList(collection, query, update);
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(RatingsEpisode entity)
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
        /// The <see cref="RatingsEpisode"/>.
        /// </returns>
        protected override RatingsEpisode HookRead(Tuple<string, int, int> id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.Item1, CollectionPrefix));
            var query = Query.And(
                Query<RatingsEpisode>.EQ(ratings => ratings.TvShowId, id.Item1),
                Query<RatingsEpisode>.EQ(ratings => ratings.SeasonNumber, id.Item2),
                Query<RatingsEpisode>.EQ(ratings => ratings.EpisodeNumber, id.Item3));

            return collection.FindOne<RatingsEpisode>(query, "_id", "Key");
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(RatingsEpisode entity)
        {
            throw new NotSupportedException("this method currently is not supported.");
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(Tuple<string, int, int> id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.Item1, CollectionPrefix));
            var query = Query.And(
                Query<RatingsEpisode>.EQ(ratings => ratings.TvShowId, id.Item1),
                Query<RatingsEpisode>.EQ(ratings => ratings.SeasonNumber, id.Item2),
                Query<RatingsEpisode>.EQ(ratings => ratings.EpisodeNumber, id.Item3));

            collection.FindAndRemove(query, SortBy.Null);
        }
    }
}
