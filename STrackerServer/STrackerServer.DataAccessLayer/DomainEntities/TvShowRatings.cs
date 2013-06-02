// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowRatings.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of television show ratings domain entity. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// The television show ratings.
    /// </summary>
    public class TvShowRatings : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowRatings"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public TvShowRatings(string key)
        {
            this.Key = key;
            this.TvShowId = key;
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
        /// Gets or sets the ratings.
        /// </summary>
        public List<Rating> Ratings { get; set; }  
    }
}