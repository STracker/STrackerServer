﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonView.cs" company="STracker">
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
    public class SeasonView
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
        /// Gets or sets the season number.
        /// </summary>
        public int SeasonNumber { get; set; }

        /// <summary>
        /// Gets or sets the episode list.
        /// </summary>
        public IEnumerable<Episode.EpisodeSynopsis> EpisodeList { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary>
        public string Poster { get; set; }

        /// <summary>
        /// Gets or sets the television show name.
        /// </summary>
        public string TvShowName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is subscribed.
        /// </summary>
        public bool IsSubscribed { get; set; }
    }
}