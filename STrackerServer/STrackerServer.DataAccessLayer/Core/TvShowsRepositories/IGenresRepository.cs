// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGenresRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The genres Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.TvShowsRepositories
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The GenresRepository interface.
    /// </summary>
    public interface IGenresRepository : IRepository<Genre, string>
    {
        /// <summary>
        /// Add television show synopsis to genre.
        /// </summary>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <param name="genre">
        /// The genre.
        /// </param>
        void AddTvShowToGenre(TvShow.TvShowSynopsis tvshow, Genre.GenreSynopsis genre);

        /// <summary>
        /// The remove television show from genre.
        /// </summary>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <param name="genre">
        /// The genre.
        /// </param>
        void RemoveTvShowFromGenre(TvShow.TvShowSynopsis tvshow, Genre.GenreSynopsis genre);
    }
}
