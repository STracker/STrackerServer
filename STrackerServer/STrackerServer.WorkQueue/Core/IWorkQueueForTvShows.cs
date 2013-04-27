// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWorkQueueForTvShows.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interfaces for work queue taht creates television shows.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.WorkQueue.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The work queue for television shows interface.
    /// </summary>
    public interface IWorkQueueForTvShows : IWorkQueue<TvShow>
    {
    }
}
