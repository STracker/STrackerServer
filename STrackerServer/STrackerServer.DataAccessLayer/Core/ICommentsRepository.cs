// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The base comments repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The CommentsRepository interface.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity key.
    /// </typeparam>
    public interface ICommentsRepository<T, in TK> : IRepository<T, TK> where T : BaseComments<TK>
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
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="userId">
        /// The user Id.
        /// </param>
        /// <param name="commentPosition">
        /// The comment Position.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveComment(TK key, string userId, int commentPosition);
    }
}
