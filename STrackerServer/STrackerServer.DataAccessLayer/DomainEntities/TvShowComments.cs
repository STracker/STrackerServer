// --------------------------------------------------------------------------------------------------------------------
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

    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// The television show comments.
    /// </summary>
    public class TvShowComments : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowComments"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public TvShowComments(string key)
        {
            this.Key = key;
            this.TvShowId = key;

            this.Comments = new List<Comment>();
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public List<Comment> Comments { get; set; }
    }
}