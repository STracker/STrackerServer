// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show ratings repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Repository.MongoDB.Core
{
    using global::MongoDB.Bson.Serialization;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The television show ratings repository.
    /// </summary>
    public class TvShowRatingsRepository : BaseRepository<TvShowRatings, string>, ITvShowRatingsRepository 
    {
        /// <summary>
        /// The collection prefix.
        /// </summary>
        private const string CollectionPrefix = "Ratings";

        /// <summary>
        /// Initializes static members of the <see cref="TvShowRatingsRepository"/> class.
        /// </summary>
        static TvShowRatingsRepository()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(BaseRatings<string>)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<BaseRatings<string>>(
                cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(c => c.Key);

                    // ignoring _id field when deserialize.
                    cm.SetIgnoreExtraElementsIsInherited(true);
                    cm.SetIgnoreExtraElements(true);
                });
            BsonClassMap.RegisterClassMap<TvShowRatings>();
        }

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
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Is abstract because his implementation is diferent from repository to repository.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookCreate(TvShowRatings entity)
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
        public override TvShowRatings HookRead(string key)
        {
            var query = Query<TvShowRatings>.EQ(r => r.TvShowId, key);

            var collection = this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix));

            var ratings = collection.FindOneAs<TvShowRatings>(query);
            if (ratings == null)
            {
                return null;
            }

            ratings.Key = key;

            return ratings;
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
        public override bool HookUpdate(TvShowRatings entity)
        {
            var query = Query<TvShowRatings>.EQ(r => r.TvShowId, entity.Key);
            var update = Update<TvShowRatings>.Set(r => r.Ratings, entity.Ratings);

            return this.Database.GetCollection(string.Format("{0}-{1}", entity.TvShowId, CollectionPrefix)).Update(query, update).Ok;
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
        public override bool HookDelete(string key)
        {
            var query = Query<TvShowRatings>.EQ(ratings => ratings.TvShowId, key);

            return this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix)).FindAndRemove(query, SortBy.Null).Ok;
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
        public bool AddRating(string key, Rating rating)
        {
            var query = Query<TvShowRatings>.EQ(ratings => ratings.TvShowId, key);

            var collection = this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix));

            return collection.Update(query, Update<TvShowRatings>.Pull(tvr => tvr.Ratings, rating)).Ok && collection.Update(query, Update<TvShowRatings>.Push(tvr => tvr.Ratings, rating)).Ok;
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
        public bool RemoveRating(string key, Rating rating)
        {
            var query = Query<TvShowRatings>.EQ(ratings => ratings.TvShowId, key);

            return this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix)).Update(query, Update<TvShowRatings>.Pull(tvr => tvr.Ratings, rating)).Ok;
        }
    }
}
