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
    using System.Collections.Generic;

    /// <summary>
    /// The television show comments.
    /// </summary>
    public class TvShowComments : BaseComments<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowComments"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public TvShowComments(string key)
            : base(key)
        {
            this.TvShowId = key;

            this.Comments = new List<Comment>();
        }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }
    }
}