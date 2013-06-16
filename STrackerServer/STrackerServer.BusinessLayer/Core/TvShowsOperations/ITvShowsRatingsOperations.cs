// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowsRatingsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over television shows ratings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core.TvShowsOperations
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The television shows ratings operations interface.
    /// </summary>
    public interface ITvShowsRatingsOperations : IRatingsOperations<RatingsTvShow, string>
    {
    }
}
