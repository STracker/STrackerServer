// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodesRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of episodes repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.EpisodesRepositories
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes repository interface.
    /// </summary>
    public interface IEpisodesRepository : IRepository<Episode, Tuple<string, int, int>>
    {
        /// <summary>
        /// Create several episodes.
        /// </summary>
        /// <param name="episodes">
        /// The episodes.
        /// </param>
        void CreateAll(IEnumerable<Episode> episodes);

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
    }
}
