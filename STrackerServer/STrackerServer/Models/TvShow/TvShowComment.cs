// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowComment.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowComment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using STrackerServer.Models.Partial;
    using STrackerServer.Models.TvShow.Options;

    /// <summary>
    /// The television show comment view.
    /// </summary>
    public class TvShowComment
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

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
        /// Gets or sets the options.
        /// </summary>
        public TvShowCommentOptions Options { get; set; }

        /// <summary>
        /// The television show edit comment options.
        /// </summary>
        public class TvShowCommentOptions
        {
            /// <summary>
            /// Gets or sets the television show id.
            /// </summary>
            public string TvShowId { get; set; }

            /// <summary>
            /// Gets or sets the poster.
            /// </summary>
            public string Poster { get; set; }
        }
    }
}