// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowsWorkItemsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interfaces for work queue that creates television shows in database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The television shows work items interface.
    /// </summary>
    public interface ITvShowsWorkItemsRepository : IRepository<TvShowWorkItem, string>
    {
    }
}
