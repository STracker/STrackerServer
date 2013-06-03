// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowCommentAddView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowCommentAddView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The comment create view.
    /// </summary>
    public class TvShowCommentAddView
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
    }
}