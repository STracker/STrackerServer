// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkResponse.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Enum for various responses of method AddWork in work queues.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.WorkQueue.Core
{
    /// <summary>
    /// The work response.
    /// </summary>
    public enum WorkResponse
    {
        /// <summary>
        /// The error.
        /// </summary>
        Error,

        /// <summary>
        /// The in process.
        /// </summary>
        InProcess
    }
}