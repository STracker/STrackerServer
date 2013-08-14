// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimilarTvShowsView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the SimilarTvShowsView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Genre
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The similar television shows view.
    /// </summary>
    public class SimilarTvShowsView
    {
        /// <summary>
        /// Gets or sets the similar television shows.
        /// </summary>
        public ICollection<TvShow.TvShowSynopsis> TvShows { get; set; }
    }
}