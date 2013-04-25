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
    /// The Work queue interface.
    /// </summary>
    public interface IWorkQueue
    {
        /// <summary>
        /// Add one work to queue.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The <see cref="WorkResponse"/>.
        /// </returns>
        WorkResponse AddWork(params object[] parameters);
    }
}