// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over episodes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core.EpisodesOperations
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes operations interface.
    /// </summary>
    public interface IEpisodesOperations : ICrudOperations<Episode, Episode.EpisodeId>
    {
        /// <summary>
        /// Get all episodes from one season.
        /// </summary>
        /// <param name="id">
        /// The id of the season.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        ICollection<Episode.EpisodeSynopsis> GetAllFromOneSeason(Season.SeasonId id);
    }
}