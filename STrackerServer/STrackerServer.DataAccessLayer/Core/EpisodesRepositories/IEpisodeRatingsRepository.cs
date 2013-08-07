// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodeRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episode ratings Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.EpisodesRepositories
{
    using System;

    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The Television Show Ratings Repository interface.
    /// </summary>
    public interface IEpisodeRatingsRepository : IRatingsRepository<RatingsEpisode, Tuple<string, int, int>>
    {
    }
}