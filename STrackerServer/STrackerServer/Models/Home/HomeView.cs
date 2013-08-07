// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Account Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Home
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The Home Web model.
    /// </summary>
    public class HomeView
    {
        /// <summary>
        /// Gets or sets the genres.
        /// </summary>
        public IEnumerable<Genre.GenreSynopsis> Genres { get; set; }

        /// <summary>
        /// Gets or sets the top rated.
        /// </summary>
        public IList<TvShow.TvShowSynopsis> TopRated { get; set; }

        /// <summary>
        /// Gets or sets the new episodes.
        /// </summary>
        public ICollection<NewTvShowEpisodes> NewEpisodes { get; set; }
    }
}