﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The Television Show ratings Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The Television Show Ratings Repository interface.
    /// </summary>
    public interface ITvShowRatingsRepository : IRatingsRepository<TvShowRatings, string>
    {
    }
}
