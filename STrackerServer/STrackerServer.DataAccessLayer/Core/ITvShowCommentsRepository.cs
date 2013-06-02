// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The Television Show BaseComments Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The Television Show BaseComments Repository interface.
    /// </summary>
    public interface ITvShowCommentsRepository : IRepository<TvShowComments, string>
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
        bool AddComment(string key, Comment comment);

        /// <summary>
        /// The remove comment.
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
        bool RemoveComment(string key, Comment comment);
    }
}
