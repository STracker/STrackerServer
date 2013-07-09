// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeWatched.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved
// </copyright>
// <summary>
//   Defines the EpisodeWatched type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Episode
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The episode watched.
    /// </summary>
    public class EpisodeWatched
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

        /// <summary>
        /// Gets or sets the episode number.
        /// </summary>
        [Required]
        public int EpisodeNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the episode was watched.
        /// </summary>
        [Required]
        public bool Watched { get; set; }

        /// <summary>
        /// Gets or sets the redirect url.
        /// </summary>
        [Required]
        public string RedirectUrl { get; set; }
    }
}