// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGenresRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The genres Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The GenresRepository interface.
    /// </summary>
    public interface IGenresRepository : IRepository<Genre, string>
    {
        /// <summary>
        /// Add television show synopsis to genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddTvShowToGenre(string genre, TvShow.TvShowSynopsis tvshow);

        /// <summary>
        /// The remove television show from genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveTvShowFromGenre(string genre, TvShow.TvShowSynopsis tvshow);

        /// <summary>
        /// Get all genres.
        /// </summary>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        ICollection<Genre.GenreSynopsis> GetAll();
    }
}