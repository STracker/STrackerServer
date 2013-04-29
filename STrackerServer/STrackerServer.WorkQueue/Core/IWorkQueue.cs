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
    /// Type of the return of work.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of the id.
    /// </typeparam>
    public interface IWorkQueue<T, TK>
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
        IWork<T, TK> AddWork(params object[] parameters);

        /// <summary>
        /// The wait for work.
        /// </summary>
        /// <param name="work">
        /// The work.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T WaitForWork(IWork<T, TK> work);

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
        IWork<T, TK> ExistsWork(object workId);
    }
}