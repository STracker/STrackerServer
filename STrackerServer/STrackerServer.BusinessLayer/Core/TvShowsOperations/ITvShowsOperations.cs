// --------------------------------------------------------------------------------------------------------------------
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
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// Television shows operations interface.
    /// </summary>
    public interface ITvShowsOperations : ICrudOperations<TvShow, string>
    {
        /// <summary>
        /// Get a list of television shows with name equals to name in parameters.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="range">
        /// The range.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        ICollection<TvShow.TvShowSynopsis> ReadByName(string name, Range range = null);

        /// <summary>
        /// Get a list of television shows with name equals to name in parameters directly to the database
        /// to avoid calls to the background worker.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        ICollection<TvShow.TvShowSynopsis> DirectReadByName(string name);
    }
}