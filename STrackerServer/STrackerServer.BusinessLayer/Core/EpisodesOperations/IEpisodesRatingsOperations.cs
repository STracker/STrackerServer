// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodesRatingsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over episodes ratings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core.EpisodesOperations
{
    using System;

    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The episodes ratings operations interface.
    /// </summary>
    public interface IEpisodesRatingsOperations : IRatingsOperations<RatingsEpisode, Tuple<string, int, int>>
    {
        /// <summary>
        /// The get average rating.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        double GetAverageRating(Tuple<string, int, int> key);
    }
}
