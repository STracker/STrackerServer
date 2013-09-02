// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utils.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The utils.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests
{
    using System.Configuration;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The test utils.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// The test database name.
        /// </summary>
        public static string DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];

        /// <summary>
        /// The get dummy television show modal.
        /// </summary>
        /// <param name="id">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public static TvShow CreateTvShowDummy(string id)
        {
            var tvshow = new TvShow(id)
            {
                Name = "Name",
                Poster = "Poster",
                Description = "Description",
                AirDay = "AirDay",
                AirTime = "AirTime",
                FirstAired = "FirstAired"
            };
            tvshow.Genres.Add(new Genre.GenreSynopsis
            {
                Id = "Test",
                Name = "Test",
                Uri = "Uri"
            });

            return tvshow;
        }
    }
}
