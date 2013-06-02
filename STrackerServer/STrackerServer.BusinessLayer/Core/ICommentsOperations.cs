// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommentsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The CommentsOperations interface.
    /// </summary>
    /// <typeparam name="TC">
    /// Type of the comment entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of the key.
    /// </typeparam>
    public interface ICommentsOperations<out TC, in TK>
    {
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
        bool AddComment(TK key, Comment comment);

        /// <summary>
        /// The get comments.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="TC"/>.
        /// </returns>
        TC GetComments(TK key);

        /// <summary>
        /// The remove comment.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        void RemoveComment(TK key, Comment comment);
    }
}