// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeRemoveComment.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episode remove comment.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Episode
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The episode remove comment.
    /// </summary>
    public class EpisodeRemoveComment
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
        /// Gets or sets the id.
        /// </summary>
        [Required]
        public string Id { get; set; }
    }
}