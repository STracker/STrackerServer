// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodeCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The Episode Comments Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using System;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The Episode Comments Repository interface.
    /// </summary>
    public interface IEpisodeCommentsRepository : IRepository<EpisodeComments, Tuple<string, int, int>>
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
        bool AddComment(Tuple<string, int, int> key, Comment comment);
    }
}
