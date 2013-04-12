// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISeasonsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of seasons repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using System;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Seasons repository interface.
    /// </summary>
    public interface ISeasonsRepository : IRepository<Season, Tuple<string, int>>
    {
        // TODO, add additional stuff...
    }
}
