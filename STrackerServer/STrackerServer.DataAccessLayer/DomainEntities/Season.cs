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
        public Season()
        {
            this.EpisodeSynopses = new List<Episode.EpisodeSynopsis>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Tuple<string, int> Id { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public Tuple<string, int> Key { get; set; }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the season number.
        /// </summary>
        public int SeasonNumber { get; set; }

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
            var uri = string.Format("tvshows/{0}/seasons/{1}", this.TvShowId, this.SeasonNumber);

            return new SeasonSynopsis { SeasonNumber = this.SeasonNumber, Uri = uri };
        }

        /// <summary>
        /// Season synopsis object.
        /// </summary>
        public class SeasonSynopsis : ISynopsis
        {
            /// <summary>
            /// Gets or sets the season number.
            /// </summary>
            public int SeasonNumber { get; set; }

            /// <summary>
            /// Gets or sets the uri.
            /// </summary>
            public string Uri { get; set; }
        }
    }
}