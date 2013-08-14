// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGenresOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Interface that defines the contract of operations over genres.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Genres Operations interface.
    /// </summary>
    public interface IGenresOperations : ICrudOperations<Genre, string>
    {
        /// <summary>
        /// Get all synopsis from all genres.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        ICollection<Genre.GenreSynopsis> ReadAllSynopsis();

        /// <summary>
        /// The get television shows with the most percentage of genres.
        /// </summary>
        /// <param name="genres">
        /// The genres.
        /// </param>
        /// <param name="excludeTvShow">
        /// The exclude television show.
        /// </param>
        /// <param name="maxtvShows">
        /// The max television shows.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        ICollection<TvShow.TvShowSynopsis> GetTvShows(ICollection<string> genres, string excludeTvShow, int maxtvShows);
    }
}