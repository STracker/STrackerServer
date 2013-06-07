// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodesRatingsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over episodes ratings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using System;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The episodes ratings operations interface.
    /// </summary>
    public interface IEpisodesRatingsOperations : IRatingsOperations<EpisodeRatings, Tuple<string, int, int>>
    {
    }
}
