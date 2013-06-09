// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowComments.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowComments type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The television show comments view.
    /// </summary>
    public class TvShowComments
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public List<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public TvShowCommentsOptions Options { get; set; }

        /// <summary>
        /// The television show comments options.
        /// </summary>
        public class TvShowCommentsOptions
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
}