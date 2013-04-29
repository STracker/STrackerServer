﻿// --------------------------------------------------------------------------------------------------------------------
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
    /// Type of the return of work.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of the id.
    /// </typeparam>
    public interface IWork<out T, out TK>
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        TK Id { get; }

        /// <summary>
        /// The begin execute work.
        /// </summary>
        void BeginExecute();

        /// <summary>
        /// The end execute work.
        /// </summary>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T EndExecute();
    }
}