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
    using System.Linq;

    using global::MongoDB.Driver;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The base comments repository.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity key.
    /// </typeparam>
    public abstract class BaseCommentsRepository<T, TK> : BaseRepository<T, TK>, ICommentsRepository<T, TK> where T : BaseComments<TK>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommentsRepository{T,TK}"/> class.
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
        }

        /// <summary>
        /// The add comment.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddComment(TK key, Comment comment)
        {
            var comments = this.Read(key);
            if (comments == null)
            {
                return false;
            }

            comments.Comments.Add(comment);
            return this.Update(comments);
        }

        /// <summary>
        /// The remove comment.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="commentPosition">
        /// The comment position.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveComment(TK key, string userId, int commentPosition)
        {
            var comments = this.Read(key);
            if (comments == null)
            {
                return false;
            }

            var comment = comments.Comments.ElementAt(commentPosition);
            if (!comment.UserId.Equals(userId))
            {
                return false;
            }

            comments.Comments.Remove(comment);
            return this.Update(comments);
        }
    }
}