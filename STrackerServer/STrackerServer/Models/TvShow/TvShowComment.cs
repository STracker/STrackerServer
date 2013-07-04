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
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary>
        public string Poster { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }
    }
}