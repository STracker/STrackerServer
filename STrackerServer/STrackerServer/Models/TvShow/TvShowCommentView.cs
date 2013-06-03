// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowCommentView.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowCommentView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    /// <summary>
    /// The television show comment view.
    /// </summary>
    public class TvShowCommentView
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
        /// Gets or sets the position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public string Body { get; set; }
    }
}