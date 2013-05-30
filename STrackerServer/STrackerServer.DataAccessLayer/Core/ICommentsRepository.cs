// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The comments repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The CommentsRepository interface.
    /// </summary>
    public interface ICommentsRepository
    {
        /// <summary>
        /// The create television show comments.
        /// </summary>
        /// <param name="comments">
        /// The comments.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool CreateTvShowComments(TvShowComments comments);

        /// <summary>
        /// The create episode comments.
        /// </summary>
        /// <param name="comments">
        /// The comments.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool CreateEpisodeComments(EpisodeComments comments);

        /// <summary>
        /// The read television show comments.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShowComments"/>.
        /// </returns>
        TvShowComments ReadTvShowComments(string tvshowId);

        /// <summary>
        /// The read episode comments.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonId">
        /// The season id.
        /// </param>
        /// <param name="episodeNumber">
        /// The episode number.
        /// </param>
        /// <returns>
        /// The <see cref="EpisodeComments"/>.
        /// </returns>
        EpisodeComments ReadEpisodeComments(string tvshowId, string seasonId, int episodeNumber);

        /// <summary>
        /// The update show comment.
        /// </summary>
        /// <param name="comments">
        /// The comments.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool UpdateShowComment(TvShowComments comments);

        /// <summary>
        /// The update episode comment.
        /// </summary>
        /// <param name="comments">
        /// The comments.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool UpdateEpisodeComment(EpisodeComments comments);
    }
}
