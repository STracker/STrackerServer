// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodesRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of episodes repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using System;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes repository interface.
    /// </summary>
    public interface IEpisodesRepository : IRepository<Episode, Tuple<string, int, int>>
    {
        // TODO, add additional stuff...
    }
}
