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
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// Season domain entity.
    /// </summary>
    public class Season : IEntity<Season.SeasonId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Season"/> class.
        /// </summary>
        public Season()
        {
            this.Episodes = new List<Episode.EpisodeSynopsis>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Season"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Season(SeasonId id) : this()
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public SeasonId Id { get; set; }

        /// <summary>
        /// Gets or sets the version of the entity.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the episodes synopses.
        /// </summary>
        public List<Episode.EpisodeSynopsis> Episodes { get; set; }

        /// <summary>
        /// Get the season synopsis.
        /// </summary>
        /// <returns>
        /// The <see cref="SeasonSynopsis"/>.
        /// </returns>
        public SeasonSynopsis GetSynopsis()
        {
            var uri = string.Format("tvshows/{0}/seasons/{1}", this.Id.TvShowId, this.Id.SeasonNumber);

            return new SeasonSynopsis
                {
                    Id = this.Id,
                    Uri = uri
                };
        }

        /// <summary>
        /// Season synopsis object.
        /// </summary>
        public class SeasonSynopsis : ISynopsis
        {
            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            public SeasonId Id { get; set; }

            /// <summary>
            /// Gets or sets the uri.
            /// </summary>
            public string Uri { get; set; }
        }

        /// <summary>
        /// The season id.
        /// </summary>
        public class SeasonId
        {
            /// <summary>
            /// Gets or sets the television show id.
            /// </summary>
            public string TvShowId { get; set; }

            /// <summary>
            /// Gets or sets the season number.
            /// </summary>
            public int SeasonNumber { get; set; }
        }
    }
}