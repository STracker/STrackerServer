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
        /// <param name="id">
        /// The id.
        /// </param>
        public Genre(string id) : this()
        {
            this.Key = id;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the television shows synopses associated to this genre.
        /// </summary>
        public List<TvShow.TvShowSynopsis> TvshowsSynopses { get; set; }

        /// <summary>
        /// The get synopsis.
        /// </summary>
        /// <returns>
        /// The <see cref="GenreSynopsis"/>.
        /// </returns>
        public GenreSynopsis GetSynopsis()
        {
            var uri = string.Format("genres/{0}", this.Key);
            return new GenreSynopsis { Id = this.Key, Uri = uri };
        }

        /// <summary>
        /// The genre synopsis.
        /// </summary>
        public class GenreSynopsis : ISynopsis
        {
            /// <summary>
            /// Gets or sets the genre type.
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets the uri.
            /// </summary>
            public string Uri { get; set; }
        }
    }
}