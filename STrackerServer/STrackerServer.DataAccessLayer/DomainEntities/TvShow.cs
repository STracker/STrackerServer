// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShow.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of television show domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// Television show domain entity.
    /// </summary>
    public class TvShow : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="STrackerServer.DataAccessLayer.DomainEntities.TvShow"/> class.
        /// </summary>
        public TvShow()
        {
            this.Actors = new List<Actor>();
            this.Genres = new List<Genre.GenreSynopsis>();
            this.SeasonSynopsis = new List<Season.SeasonSynopsis>();    
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShow"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public TvShow(string id) : this()
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the version of the entity.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary>
        public string Poster { get; set; }

        /// <summary>
        /// Gets or sets the first aired.
        /// </summary>
        public string FirstAired { get; set; }

        /// <summary>
        /// Gets or sets the air day.
        /// </summary>
        public string AirDay { get; set; }

        /// <summary>
        /// Gets or sets the air time.
        /// </summary>
        public string AirTime { get; set; }

        /// <summary>
        /// Gets or sets the runtime.
        /// </summary>
        public int Runtime { get; set; }

        /// <summary>
        /// Gets or sets the actors.
        /// </summary>
        public List<Actor> Actors { get; set; }

        /// <summary>
        /// Gets or sets the genres.
        /// </summary>
        public List<Genre.GenreSynopsis> Genres { get; set; }

        /// <summary>
        /// Gets or sets the season synopses.
        /// </summary>
        public List<Season.SeasonSynopsis> SeasonSynopsis { get; set; }

        /// <summary>
        /// Get the television show synopsis.
        /// </summary>
        /// <returns>
        /// The <see cref="TvShowSynopsis"/>.
        /// </returns>
        public TvShowSynopsis GetSynopsis()
        {
            var uri = string.Format("tvshows/{0}", this.Id);
            
            return new TvShowSynopsis { Id = this.Id, Name = this.Name, Poster = this.Poster, Uri = uri };
        }

        /// <summary>
        /// Television show synopsis object.
        /// </summary>
        public class TvShowSynopsis : ISynopsis
        {
            /// <summary>
            /// Gets or sets the television show id.
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the poster.
            /// </summary>
            public string Poster { get; set; }

            /// <summary>
            /// Gets or sets the uri.
            /// </summary>
            public string Uri { get; set; }
        }
    }
}