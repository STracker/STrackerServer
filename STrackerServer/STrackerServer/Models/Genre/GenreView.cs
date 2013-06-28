// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenreView.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The genre view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Genre
{
    using System.Collections.Generic;

    /// <summary>
    /// The genre view.
    /// </summary>
    public class GenreView
    {
        /// <summary>
        /// Gets or sets the genre name.
        /// </summary>
        public string GenreName { get; set; }

        /// <summary>
        /// Gets or sets the television shows.
        /// </summary>
        public List<DataAccessLayer.DomainEntities.TvShow.TvShowSynopsis> TvShows { get; set; }
    }
}