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
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    /// <summary>
    /// The BaseCommentsOperations interface.
    /// </summary>
    /// <typeparam name="TC">
    /// Type of the comment entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of the key.
    /// </typeparam>
    public interface ICommentsOperations<TC, in TK> : ICrudOperations<TC, TK> where TC : CommentsBase<TK>
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
        /// The remove comment.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="userId">
        /// The user Id.
        /// </param>
        /// <param name="commentId">
        /// The comment Id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveComment(TK id, string userId, string commentId);
    }
}