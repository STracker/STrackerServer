// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueueForCreateTvShowsWorks.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IQueue interface for create tvshows works.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.WorkQueue
{
    using System.Collections.Concurrent;

    using STrackerServer.WorkQueue.Core;

    /// <summary>
    /// The queue for create television shows works.
    /// </summary>
    public class QueueForCreateTvShowsWorks : IQueue<string, CreateTvShowsWork>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueForCreateTvShowsWorks"/> class.
        /// </summary>
        public QueueForCreateTvShowsWorks()
        {
            this.Queue = new ConcurrentDictionary<string, CreateTvShowsWork>();
        }

        /// <summary>
        /// Gets the queue.
        /// </summary>
        public ConcurrentDictionary<string, CreateTvShowsWork> Queue { get; private set; }
    }
}
