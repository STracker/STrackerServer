// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISeasonsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of seasons repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Seasons repository interface.
    /// </summary>
    public interface ISeasonsRepository : IRepository<Season, Tuple<string, int>>
    {
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
        IEnumerable<Season.SeasonSynopsis> GetAllFromOneTvShow(string tvshowId);

        /// <summary>
        /// Create several seasons.
        /// </summary>
        /// <param name="seasons">
        /// The seasons.
        /// </param>
        void CreateAll(IEnumerable<Season> seasons);
    }
}
