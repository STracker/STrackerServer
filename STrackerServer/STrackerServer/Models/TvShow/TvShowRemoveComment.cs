// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowRemoveComment.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowRemoveComment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The television show comment remove.
    /// </summary>
    public class TvShowRemoveComment
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        [Required]
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Required]
        public string Id { get; set; }
    }
}