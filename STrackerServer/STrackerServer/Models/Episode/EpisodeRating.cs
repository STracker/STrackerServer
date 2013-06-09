// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeRating.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episode rating.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Episode
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The episode rating.
    /// </summary>
    public class EpisodeRating
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
        /// Gets or sets the rating.
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary>
        public string Poster { get; set; }
    }
}