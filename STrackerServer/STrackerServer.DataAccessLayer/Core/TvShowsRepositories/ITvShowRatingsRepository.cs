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
        /// The get top rated.
        /// </summary>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        ICollection<TvShow.TvShowSynopsis> GetTopRated(int max);
    }
}
