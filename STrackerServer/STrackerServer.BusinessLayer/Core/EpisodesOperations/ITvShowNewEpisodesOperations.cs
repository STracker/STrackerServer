// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowNewEpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Interface that defines the contract of operations over television show new episodes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core.EpisodesOperations
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The interface of new episodes operations.
    /// </summary>
    public interface ITvShowNewEpisodesOperations
    {
        /// <summary>
        /// Get the new episodes from one television show up to the date in parameters.
        /// If the date is null, return all new episodes from the television show.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="NewTvShowEpisodes"/>.
        /// </returns>
        NewTvShowEpisodes GetNewEpisodes(string tvshowId, DateTime? date);

        /// <summary>
        /// Get new episodes from all television shows.
        /// If the date is null, return all new episodes from all television shows.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        ICollection<NewTvShowEpisodes> GetNewEpisodes(DateTime? date);
    }
}