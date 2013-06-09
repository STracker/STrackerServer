﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over television shows.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core.TvShowsOperations
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Television shows operations interface.
    /// </summary>
    public interface ITvShowsOperations : ICrudOperations<TvShow, string>
    {
        /// <summary>
        /// The read by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        List<TvShow.TvShowSynopsis> ReadByName(string name);
    }
}