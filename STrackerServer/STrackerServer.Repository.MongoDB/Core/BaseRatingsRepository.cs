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

    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;
    using STrackerServer.Logger.Core;

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
        /// <param name="logger">
        /// The logger.
        /// </param>
        protected BaseRatingsRepository(MongoClient client, MongoUrl url, ILogger logger)
            : base(client, url, logger)
        {
            this.CollectionPrefix = ConfigurationManager.AppSettings["RatingsCollection"];
        }
    }
}