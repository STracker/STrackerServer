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
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    /// <summary>
    /// The CommentsRepository interface.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity key.
    /// </typeparam>
    public interface ICommentsRepository<T, in TK> : IRepository<T, TK> where T : CommentsBase<TK>
    {
        /// <summary>
        /// Add one comment.
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
        bool AddComment(TK id, Comment comment);

        /// <summary>
        /// Remove one comment.
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
        bool RemoveComment(TK id, Comment comment);
    }
}