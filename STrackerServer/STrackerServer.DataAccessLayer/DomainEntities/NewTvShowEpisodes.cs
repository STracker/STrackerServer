// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewTvShowEpisodes.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of weekly episodes domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// The weekly episodes.
    /// </summary>
    public class NewTvShowEpisodes : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewTvShowEpisodes"/> class.
        /// </summary>
        public NewTvShowEpisodes()
        {
            this.Episodes = new List<Episode.EpisodeSynopsis>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewTvShowEpisodes"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public NewTvShowEpisodes(string id)
            : this()
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the id. In this case the id is the television show id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the version of the entity.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the television show.
        /// </summary>
        public TvShow.TvShowSynopsis TvShow { get; set; }

        /// <summary>
        /// Gets or sets the episodes.
        /// </summary>
        public List<Episode.EpisodeSynopsis> Episodes { get; set; } 
    }
}
