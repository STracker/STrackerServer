// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonWeb.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Season Web View Model
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Season
{
    using System.Collections.Generic;
    using DataAccessLayer.DomainEntities;

    /// <summary>
    /// The season web.
    /// </summary>
    public class SeasonWeb
    {
        /// <summary>
        /// Gets or sets the artwork.
        /// </summary>
        public string Artwork { get; set; }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the episode list.
        /// </summary>
        public IEnumerable<Episode.EpisodeSynopsis> EpisodeList { get; set; }
    }
}