﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISeasonsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over seasons.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.BusinessLayer.Core
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Seasons operations interface.
    /// </summary>
    public interface ISeasonsOperations : IAsyncOperations<Season, Tuple<string, int>>
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
    }
}