﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowsRatingsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over television shows ratings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core.TvShowsOperations
{
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The television shows ratings operations interface.
    /// </summary>
    public interface ITvShowsRatingsOperations : IRatingsOperations<RatingsTvShow, string>
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
        double GetAverageRating(string key);
    }
}
