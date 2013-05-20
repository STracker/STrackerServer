// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Genre.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of genre domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// The genre.
    /// </summary>
    public class Genre : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Genre"/> class.
        /// </summary>
        public Genre()
        {
            this.TvshowsSynopses = new List<TvShow.TvShowSynopsis>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Genre"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the genre.
        /// </param>
        public Genre(string name)
        {
            this.Key = name;
            this.TvshowsSynopses = new List<TvShow.TvShowSynopsis>();
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get;  set; }

        /// <summary>
        /// Gets or sets the television shows synopses.
        /// </summary>
        public List<TvShow.TvShowSynopsis> TvshowsSynopses { get; set; }  
    }
}