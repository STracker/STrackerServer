// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowPartialOptions.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show options.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    /// <summary>
    /// The television show options.
    /// </summary>
    public class TvShowPartialOptions
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the television show poster.
        /// </summary>
        public string Poster { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is subscribed.
        /// </summary>
        public bool IsSubscribed { get; set; }

        /// <summary>
        /// Gets or sets the unsubscribe redirect url.
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}