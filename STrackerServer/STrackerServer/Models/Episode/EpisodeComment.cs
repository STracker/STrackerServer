﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeComment.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the EpisodeComment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Episode
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The episode comment.
    /// </summary>
    public class EpisodeComment
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
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the Timestamp.
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary>
        public string Poster { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Required]
        public string Id { get; set; }
    }
}