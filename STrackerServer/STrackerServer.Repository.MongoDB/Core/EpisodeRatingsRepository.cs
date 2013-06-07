// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the EpisodeRatingsRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The episode ratings repository.
    /// </summary>
    public class EpisodeRatingsRepository : BaseRepository<RatingsEpisode, Tuple<string, int, int>>, IEpisodeRatingsRepository 
    {
        /// <summary>
        /// The collection prefix.
        /// </summary>
        private const string CollectionPrefix = "Ratings";

        /// <summary>
        /// Initializes static members of the <see cref="EpisodeRatingsRepository"/> class.
        /// </summary>
        static EpisodeRatingsRepository()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(BaseRatings<Tuple<string, int, int>>)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<BaseRatings<Tuple<string, int, int>>>(
                cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(c => c.Key);

                    // ignoring _id field when deserialize.
                    cm.SetIgnoreExtraElementsIsInherited(true);
                    cm.SetIgnoreExtraElements(true);
                });
            BsonClassMap.RegisterClassMap<RatingsEpisode>();
        }

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
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Is abstract because his implementation is diferent from repository to repository.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookCreate(RatingsEpisode entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.TvShowId, CollectionPrefix));

            // Ensure index.
            collection.EnsureIndex(new IndexKeysBuilder().Ascending("TvShowId", "SeasonNumber", "EpisodeNumber"), IndexOptions.SetUnique(true));

            return collection.Insert(entity).Ok;
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>T</cref>
        ///     </see> .
        /// </returns>
        public override RatingsEpisode HookRead(Tuple<string, int, int> key)
        {
            var query = Query.And(
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item1, key.Item1),
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item2, key.Item2),
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item3, key.Item3));

            var rating = this.Database.GetCollection(string.Format("{0}-{1}", key.Item1, CollectionPrefix)).FindOneAs<RatingsEpisode>(query);
            if (rating == null)
            {
                return null;
            }

            rating.Key = key;

            return rating;
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Is abstract because his implementation is diferent from repository to repository.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookUpdate(RatingsEpisode entity)
        {
            var query = Query.And(
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item1, entity.Key.Item1),
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item2, entity.Key.Item2),
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item3, entity.Key.Item3));

            var update = Update<RatingsEpisode>.Set(ratings => ratings.Ratings, entity.Ratings);

            return
                this.Database.GetCollection(string.Format("{0}-{1}", entity.Key.Item1, CollectionPrefix)).Update(
                    query, update).Ok;
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// Is abstract because his implementation is diferent from repository to repository.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookDelete(Tuple<string, int, int> key)
        {
            var query = Query.And(
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item1, key.Item1),
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item2, key.Item2),
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item3, key.Item3));

            return
                this.Database.GetCollection(string.Format("{0}-{1}", key.Item1, CollectionPrefix)).FindAndRemove(
                    query, SortBy.Null).Ok;
        }

        /// <summary>
        /// The add rating.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddRating(Tuple<string, int, int> key, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", key.Item1, CollectionPrefix));

            var query = Query.And(
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item1, key.Item1),
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item2, key.Item2),
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item3, key.Item3));

            return collection.Update(query, Update<RatingsEpisode>.Pull(er => er.Ratings, rating).Push(er => er.Ratings, rating)).Ok;
        }

        /// <summary>
        /// The remove rating.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveRating(Tuple<string, int, int> key, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", key.Item1, CollectionPrefix));

            var query = Query.And(
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item1, key.Item1),
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item2, key.Item2),
                Query<RatingsEpisode>.EQ(ratings => ratings.Key.Item3, key.Item3));

            return collection.Update(query, Update<RatingsEpisode>.Pull(er => er.Ratings, rating)).Ok;
        }
    }
}
