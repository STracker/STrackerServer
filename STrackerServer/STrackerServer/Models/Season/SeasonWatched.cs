// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonWatched.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved
// </copyright>
// <summary>
//   Defines the SeasonWatched type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Season
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The season watched.
    /// </summary>
    public class SeasonWatched
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        [Required]
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the season number.
        /// </summary>
        [Required]
        public int SeasonNumber { get; set; }
    }
}