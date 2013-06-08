// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowCreateComment.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowCreateComment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using System.ComponentModel.DataAnnotations;

    using STrackerServer.Models.Partial;

    /// <summary>
    /// The television show comment create.
    /// </summary>
    public class TvShowCreateComment
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        [Required]
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        [Required]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public TvShowCreateCommentOptions Options { get; set; }
    }
}