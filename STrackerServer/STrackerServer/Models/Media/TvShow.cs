// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShow.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Models.Media
{
    using System;
    using System.Collections.Generic;
    using STrackerServer.Models.Person;
    using STrackerServer.Models.Utils;

    /// <summary>
    /// The television show.
    /// </summary>
    public class TvShow : Media
    {
        /// <summary>
        /// Gets or sets the first aired.
        /// </summary>
        public DateTime FirstAired { get; set; }

        /// <summary>
        /// Gets or sets the runtime.
        /// </summary>
        public int Runtime { get; set; }

        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        public Person Creator { get; set; }

        /// <summary>
        /// Gets or sets the genres.
        /// </summary>
        public List<Genre> Genres { get; set; } 
    }
}