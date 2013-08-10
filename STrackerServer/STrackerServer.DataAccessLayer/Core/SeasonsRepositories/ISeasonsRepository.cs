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
    public interface ISeasonsRepository : IRepository<Season, Season.SeasonId>
    {
        /// <summary>
        /// Create several seasons.
        /// </summary>
        /// <param name="seasons">
        /// The seasons.
        /// </param>
        void CreateAll(ICollection<Season> seasons);

        /// <summary>
        /// Add one episode synopsis to season's episodes list.
        /// </summary>
        /// <param name="id">
        /// The id of the season.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddEpisode(Season.SeasonId id, Episode.EpisodeSynopsis episode);

        /// <summary>
        /// Remove one episode synopsis from season's episodes list.
        /// </summary>
        /// <param name="id">
        /// The id of the season.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveEpisode(Season.SeasonId id, Episode.EpisodeSynopsis episode);
    }
}