// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Season.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of season domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Models
{
    using System.Collections.Generic;

    using STrackerServerDatabase.Core;

    /// <summary>
    /// Season domain entity.
    /// </summary>
    public class Season : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get;  set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public int Number { get;  set; }

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
            return new SeasonSynopsis { Number = this.Number };
        }

        /// <summary>
        /// Season synopsis object.
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