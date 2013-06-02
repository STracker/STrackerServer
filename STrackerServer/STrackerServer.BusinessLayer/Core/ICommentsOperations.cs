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
    public interface ICommentsOperations
    {
        /// <summary>
        /// The add comment.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddTvShowComment(string tvshowId, Comment comment);

        /// <summary>
        /// The get comments.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShowComments"/>.
        /// </returns>
        TvShowComments GetTvShowComments(string tvshowId);

        /// <summary>
        /// The add comment.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="episodeNumber">
        /// The episode number.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddEpisodeComment(string tvshowId, int seasonNumber, int episodeNumber, Comment comment);

        /// <summary>
        /// The get comments.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="episodeNumber">
        /// The episode number.
        /// </param>
        /// <returns>
        /// The <see cref="EpisodeComments"/>.
        /// </returns>
        EpisodeComments GetEpisodeComments(string tvshowId, int seasonNumber, int episodeNumber);
    }
}
