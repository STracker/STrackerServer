// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over episodes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes operations interface.
    /// </summary>
    public interface IEpisodesOperations : ICrudOperations<Episode, Tuple<string, int, int>>
    {
        /// <summary>
        /// The get all from one season.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        IEnumerable<Episode.EpisodeSynopsis> GetAllFromOneSeason(string tvshowId, int seasonNumber);

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
        bool AddComment(string tvshowId, int seasonNumber, int episodeNumber, Comment comment);

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
        EpisodeComments GetComments(string tvshowId, int seasonNumber, int episodeNumber);
    }
}