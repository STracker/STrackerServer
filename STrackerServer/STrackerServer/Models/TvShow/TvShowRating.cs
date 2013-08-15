// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowRating.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The rating view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The rating view.
    /// </summary>
    public class TvShowRating
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the television show name.
        /// </summary> 
        public string TvShowName { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary> 
        public string Poster { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary> 
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the ratings count.
        /// </summary> 
        public int Count { get; set; }
    }
}