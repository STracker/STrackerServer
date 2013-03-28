// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Season.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Models.Media
{
    using System.Collections.Generic;
    using STrackerServer.Models.Utils;

    /// <summary>
    /// The season.
    /// </summary>
    public class Season
    {
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary>
        public Artwork Poster { get; set; }

        /// <summary>
        /// Gets or sets the episodes.
        /// </summary>
        public IDictionary<int, Episode> Episodes { get; set; } 
    }
}