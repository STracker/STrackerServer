// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWorkQueue.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interfaces for work queue.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.WorkQueue.Core
{
    /// <summary>
    /// The  work queue interface.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the return of the work.
    /// </typeparam>
    public interface IWorkQueue<out T>
    {
        /// <summary>
        /// Add one work to queue.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IWork</cref>
        ///     </see> .
        /// </returns>
        IWork<T> AddWork(params object[] parameters);

        /// <summary>
        /// Verify if already exists the work.
        /// </summary>
        /// <param name="workId">
        /// The work id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IWork</cref>
        ///     </see> .
        /// </returns>
        IWork<T> ExistsWork(object workId);
    }
}