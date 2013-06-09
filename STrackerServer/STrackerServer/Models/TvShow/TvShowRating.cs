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
    /// <summary>
    /// The rating view.
    /// </summary>
    public class TvShowRating
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
        /// Gets or sets the options.
        /// </summary>
        public TvShowRatingOptions Options { get; set; }

        /// <summary>
        /// The television show rating options.
        /// </summary>
        public class TvShowRatingOptions
        {
            /// <summary>
            /// Gets or sets the television show id.
            /// </summary>
            public string TvShowId { get; set; }

            /// <summary>
            /// Gets or sets the television show name.
            /// </summary>
            public string TvShowName { get; set; }
        }
    }
}