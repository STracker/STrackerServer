// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Season.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Domain
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.Core;

    /// <summary>
    /// The season domain entity.
    /// </summary>
    public class Season : IEntity<Tuple<string, int>>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Tuple<string, int> Id { get; set; }

        /// <summary>
        /// Gets or sets the episode synopses.
        /// </summary>
        public List<Episode.EpisodeSynopsis> EpisodeSynopses { get; set; }  

        /// <summary>
        /// Get the season synopsis.
        /// </summary>
        /// <returns>
        /// The <see cref="SeasonSynopsis"/>.
        /// </returns>
        public SeasonSynopsis GetSynopsis()
        {
            return new SeasonSynopsis { Number = this.Id.Item2 };
        }

        /// <summary>
        /// The season synopsis object.
        /// </summary>
        public class SeasonSynopsis
        {
            /// <summary>
            /// Gets or sets the number.
            /// </summary>
            public int Number { get; set; }
        }
    }
}