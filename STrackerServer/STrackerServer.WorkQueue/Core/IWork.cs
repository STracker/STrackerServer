// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWork.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface for Works.
//  of the work.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.WorkQueue.Core
{
    /// <summary>
    /// The Work interface.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the return in End execute work method.
    /// </typeparam>
    public interface IWork<out T>
    {
        /// <summary>
        /// The begin execute work.
        /// </summary>
        void BeginExecuteWork();

        /// <summary>
        /// The end execute work.
        /// </summary>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T EndExecuteWork();
    }
}
