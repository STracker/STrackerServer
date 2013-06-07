// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The Television Show ratings Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.TvShowsRepositories
{
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The Television Show Ratings Repository interface.
    /// </summary>
    public interface ITvShowRatingsRepository : IRatingsRepository<RatingsTvShow, string>
    {
    }
}
