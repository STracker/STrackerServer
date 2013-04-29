// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueue.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface for queues.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.WorkQueue.Core
{
    using System.Collections.Concurrent;

    /// <summary>
    /// The Queue interface.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the object.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of the key.
    /// </typeparam>
    public interface IQueue<T, TK>
    {
        /// <summary>
        /// Gets the queue.
        /// </summary>
        ConcurrentDictionary<T, TK> Queue { get; } 
    }
}