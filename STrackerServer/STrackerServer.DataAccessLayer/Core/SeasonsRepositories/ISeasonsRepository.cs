// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISeasonsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of seasons repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.SeasonsRepositories
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Seasons repository interface.
    /// </summary>
    public interface ISeasonsRepository : IRepository<Season, Season.SeasonKey>
    {
        /// <summary>
        /// Create several seasons.
        /// </summary>
        /// <param name="seasons">
        /// The seasons.
        /// </param>
        void CreateAll(ICollection<Season> seasons);

        /// <summary>
        /// Get all seasons synopsis from one television show.
        /// </summary>
        /// <param name="tvshowId">
        /// Television show id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        ICollection<Season.SeasonSynopsis> GetAllFromOneTvShow(string tvshowId);

        /// <summary>
        /// The add episode synopsis.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddEpisodeSynopsis(Season.SeasonKey id, Episode.EpisodeSynopsis episode);

        /// <summary>
        /// The remove episode synopsis.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveEpisodeSynopsis(Season.SeasonKey id, Episode.EpisodeSynopsis episode);
    }
}
