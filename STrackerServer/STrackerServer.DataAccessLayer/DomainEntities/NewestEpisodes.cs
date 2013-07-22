// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewestEpisodes.cs" company="STracker">
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
    public class NewestEpisodes : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewestEpisodes"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public NewestEpisodes(string key)
        {
            this.Key = key;
            this.Episodes = new List<Episode.EpisodeSynopsis>();
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

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
