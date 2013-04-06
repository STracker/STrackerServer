// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShow.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Models.Domain
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.Core;

    /// <summary>
    /// The television show domain entity.
    /// </summary>
    public class TvShow : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the air day.
        /// </summary>
        public DateTime AirDay { get; set; }

        /// <summary>
        /// Gets or sets the runtime.
        /// </summary>
        public int Runtime { get; set; }

        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        public Person Creator { get; set; }

        /// <summary>
        /// Gets or sets the season synopses.
        /// </summary>
        public List<Season.SeasonSynopsis> SeassonSynopses { get; set; } 
    }
}