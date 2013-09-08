// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of television shows repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.TvShowsRepositories
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// Television shows repository interface.
    /// </summary>
    public interface ITvShowsRepository : IRepository<TvShow, string>
    {
        /// <summary>
        /// Get one television show by is name.
        /// </summary>
        /// <param name="name">
        /// The name of the television show.
        /// </param>
        /// <param name="range">
        /// The range.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        ICollection<TvShow.TvShowSynopsis> ReadByName(string name, Range range);

        /// <summary>
        /// Add one season to television show's seasons synopsis.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="season">
        /// The season.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddSeason(string tvshowId, Season.SeasonSynopsis season);

        /// <summary>
        /// Remove one season from television show's seasons synopsis.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="season">
        /// The season.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveSeason(string tvshowId, Season.SeasonSynopsis season);
    }
}