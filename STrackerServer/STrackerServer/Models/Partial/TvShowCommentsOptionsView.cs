// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowCommentsOptionsView.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show comments options.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Models.Partial
{
    using System.Linq;

    /// <summary>
    /// The television show comments options.
    /// </summary>
    public class TvShowCommentsOptionsView
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the television show name.
        /// </summary>
        public string TvShowName { get; set; }

        /// <summary>
        /// Gets or sets the television show poster.
        /// </summary>
        public string Poster { get; set; }
    }
}