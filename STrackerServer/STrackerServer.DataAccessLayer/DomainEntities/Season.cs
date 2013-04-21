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
            this.Key = key;
            this.TvShowId = key.Item1;
            this.SeasonNumber = key.Item2;

            this.EpisodeSynopses = new List<Episode.EpisodeSynopsis>();
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public Tuple<string, int> Key { get; set; }

        /// <summary>
        /// Gets the television show id.
        /// </summary>
        public string TvShowId { get;  private set; }

        /// <summary>
        /// Gets the season number.
        /// </summary>
        public int SeasonNumber { get;  private set; }

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
            return new SeasonSynopsis { SeasonNumber = this.SeasonNumber };
        }

        /// <summary>
        /// Season synopsis object.
        /// </summary>
        public class SeasonSynopsis
        {
            /// <summary>
            /// Gets or sets the season number.
            /// </summary>
            public int SeasonNumber { get; set; }
        }
    }
}