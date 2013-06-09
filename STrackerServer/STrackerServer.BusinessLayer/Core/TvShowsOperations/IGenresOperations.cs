// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGenresOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The GenresOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core.TvShowsOperations
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The Genres Operations interface.
    /// </summary>
    public interface IGenresOperations : ICrudOperations<Genre, string>
    {
        /// <summary>
        /// Get all genres.
        /// </summary>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        List<Genre.GenreSynopsis> GetAll();
    }
}
