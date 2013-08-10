// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of base comments repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System.Configuration;

    using global::MongoDB.Driver;

    using STrackerServer.DataAccessLayer.DomainEntities.Comments;
    using STrackerServer.Logger.Core;

    /// <summary>
    /// The base comments repository.
    /// </summary>
    /// <typeparam name="TC">
    /// Type of comment.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of comment key.
    /// </typeparam>
    public abstract class BaseCommentsRepository<TC, TK> : BaseRepository<TC, TK> where TC : CommentsBase<TK>
    {
        /// <summary>
        /// The collection prefix.
        /// </summary>
        protected readonly string CollectionPrefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommentsRepository{TC,TK}"/> class.
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
        protected BaseCommentsRepository(MongoClient client, MongoUrl url, ILogger logger)
            : base(client, url, logger)
        {
            this.CollectionPrefix = ConfigurationManager.AppSettings["CommentsCollection"];
        }
    }
}
