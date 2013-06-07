// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowsRatingsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over television shows ratings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The television shows ratings operations interface.
    /// </summary>
    public interface ITvShowsRatingsOperations : IRatingsOperations<TvShowRatings, string>
    {
    }
}
