// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Season.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of season domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// Season domain entity.
    /// </summary>
    public class Season : IEntity<Tuple<string, int>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Season"/> class.
        /// </summary>
        /// <param name="key">
        /// Compound key. Order: television show id, season number.
        /// </param>
        public Season(Tuple<string, int> key)
        {
            this.Key           = key;
            this.TvShowId = key.Item1;
            this.Number     = key.Item2;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Tuple<string, int> Key { get; set; }

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