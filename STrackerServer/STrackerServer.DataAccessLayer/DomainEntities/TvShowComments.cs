﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowComments.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    /// <summary>
    /// The television show comments.
    /// </summary>
    public class TvShowComments : Container<string, Comment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowComments"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public TvShowComments(string key)
            : base(key, "Comments")
        {
            this.TvShowId = key;
        }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }
    }
}
