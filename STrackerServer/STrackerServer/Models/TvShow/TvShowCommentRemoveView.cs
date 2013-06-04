// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowCommentRemoveView.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowCommentRemoveView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The television show comment remove view.
    /// </summary>
    public class TvShowCommentRemoveView
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        [Required]
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the Timestamp.
        /// </summary>
        [Required]
        public string Timestamp { get; set; }
    }
}