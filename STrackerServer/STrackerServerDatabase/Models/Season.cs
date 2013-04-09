// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Season.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Models
{
    using System;
    using System.Collections.Generic;

    using STrackerServerDatabase.Core;

    /// <summary>
    /// The season domain entity.
    /// </summary>
    public class Season : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Season"/> class.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        public Season(string tvshowId, int number)
        {
            this.Id = string.Format("{0}_{1}", tvshowId, number);
            this.TvShowId = tvshowId;
            this.Number = number;
            this.EpisodeSynopses = new List<Episode.EpisodeSynopsis>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Season"/> class.
        /// </summary>
        public Season()
        {    
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets the television show id.
        /// </summary>
        public string TvShowId { get; private set; }

        /// <summary>
        /// Gets the number.
        /// </summary>
        public int Number { get; private set; }

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