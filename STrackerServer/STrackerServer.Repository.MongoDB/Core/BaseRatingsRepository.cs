// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of base ratings repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System.Configuration;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The base ratings repository.
    /// </summary>
    /// <typeparam name="TR">
    /// Type of the rating.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of the rating key.
    /// </typeparam>
    public abstract class BaseRatingsRepository<TR, TK> : BaseRepository<TR, TK> where TR : RatingsBase<TK>
    {
        /// <summary>
        /// The collection prefix.
        /// </summary>
        protected readonly string CollectionPrefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRatingsRepository{TR,TK}"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        protected BaseRatingsRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
            this.CollectionPrefix = ConfigurationManager.AppSettings["RatingsCollection"];
        }

        /// <summary>
        /// The drop comments.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public void DropRatings(string id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", this.CollectionPrefix, id));
            collection.Drop();
        }

        /// <summary>
        /// The setup indexes.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        protected void SetupIndexes(MongoCollection collection)
        {
            // Ensure index.
            collection.EnsureIndex(new IndexKeysBuilder().Ascending("TvShowId", "SeasonNumber", "EpisodeNumber"), IndexOptions.SetUnique(true));
        }
    }
}