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
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The Television Show Ratings Repository interface.
    /// </summary>
    public interface ITvShowRatingsRepository : IRatingsRepository<RatingsTvShow, string>
    {
        /// <summary>
        /// Get the top rated television shows in STracker.
        /// </summary>
        /// <param name="max">
        /// The limit of television shows, in the other words, the number of the top.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        ICollection<TvShow.TvShowSynopsis> ReadTopRated(int max);
    }
}
