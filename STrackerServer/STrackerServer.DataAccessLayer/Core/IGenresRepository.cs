// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGenresRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Definition of genres Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Genres Repository interface.
    /// </summary>
    public interface IGenresRepository : IRepository<Genre, string>
    {
        /// <summary>
        /// Return a collection with all synopsis of all genres available in STracker.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        ICollection<Genre.GenreSynopsis> ReadAllSynopsis();
 
        /// <summary>
        /// Add one television show to genre.
        /// </summary>
        /// <param name="genre">
        /// The genre name.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddTvShow(string genre, TvShow.TvShowSynopsis tvshow);

        /// <summary>
        /// Remove one television show from genre.
        /// </summary>
        /// <param name="genre">
        /// The genre name.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveTvShow(string genre, TvShow.TvShowSynopsis tvshow);
    }
}