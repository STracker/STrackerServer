// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonOptionsView.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the SeasonOptionsView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Season.Options
{
    /// <summary>
    /// The season options view.
    /// </summary>
    public class SeasonOptionsView
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the television show name.
        /// </summary>
        public string TvShowName { get; set; }

        /// <summary>
        /// Gets or sets the television show poster.
        /// </summary>
        public string Poster { get; set; }
    }
}