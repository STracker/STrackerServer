// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of television shows repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Television shows repository interface.
    /// </summary>
    public interface ITvShowsRepository : IRepository<TvShow, string>
    {
        /// <summary>
        /// Get all television shows with genre equals to argument genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        List<TvShow> GetAllByGenre(Genre genre);

        // TODO, add additional stuff...
    }
}
