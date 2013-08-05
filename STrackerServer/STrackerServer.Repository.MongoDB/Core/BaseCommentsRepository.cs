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
    using System;
    using System.Configuration;

    using global::MongoDB.Driver;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
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
    public abstract class BaseCommentsRepository<TC, TK> : BaseRepository<TC, TK>, ICommentsRepository<TC, TK>
        where TC : CommentsBase<TK>
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
        /// The remove all comments.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveAllComments(TK id)
        {
            try
            {
                var collection = this.Database.GetCollection(string.Format("{0}-{1}", this.CollectionPrefix, id));
                collection.Drop();
                return true;
            }
            catch (Exception)
            {
                // TODO, add to log mechanism.
                return false;
            }
        }

        /// <summary>
        /// The add comment.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract bool AddComment(TK id, Comment comment);

        /// <summary>
        /// The remove comment.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract bool RemoveComment(TK id, Comment comment);
    }
}
