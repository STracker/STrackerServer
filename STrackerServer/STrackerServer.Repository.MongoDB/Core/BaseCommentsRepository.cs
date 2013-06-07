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

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

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
        protected BaseCommentsRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
            this.CollectionPrefix = ConfigurationManager.AppSettings["CommentsCollection"];
        }

        /// <summary>
        /// The drop comments.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public void DropComments(string id)
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
