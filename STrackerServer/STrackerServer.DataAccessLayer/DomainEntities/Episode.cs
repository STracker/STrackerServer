﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Episode.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of episode domain entity. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// CommentsEpisode domain entity.
    /// </summary>
    public class Episode : IEntity<Tuple<string, int, int>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="STrackerServer.DataAccessLayer.DomainEntities.Episode"/> class.
        /// </summary>
        public Episode()
        {  
            this.Directors = new List<Person>();
            this.GuestActors = new List<Actor>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Tuple<string, int, int> Id { get; set; }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the season number.
        /// </summary>
        public int SeasonNumber { get; set; }

        /// <summary>
        /// Gets or sets the episode number.
        /// </summary>
        public int EpisodeNumber { get; set; }

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
        public Artwork Poster { get; set; }

        /// <summary>
        /// Gets or sets the directors.
        /// </summary>
        public List<Person> Directors { get; set; }

        /// <summary>
        /// Gets or sets the guest actors.
        /// </summary>
        public List<Actor> GuestActors { get; set; }

        /// <summary>
        /// Get the episode synopsis.
        /// </summary>
        /// <returns>
        /// The <see cref="EpisodeSynopsis"/>.
        /// </returns>
        public EpisodeSynopsis GetSynopsis()
        {
            var uri = string.Format("tvshows/{0}/seasons/{1}/episodes/{2}", this.TvShowId, this.SeasonNumber, this.EpisodeNumber);

            return new EpisodeSynopsis { EpisodeNumber = this.EpisodeNumber, Name = this.Name, Uri = uri };
        }

        /// <summary>
        /// CommentsEpisode synopsis object.
        /// </summary>
        public class EpisodeSynopsis : ISynopsis
        {
            /// <summary>
            /// Gets or sets the episode number.
            /// </summary>
            public int EpisodeNumber { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the uri.
            /// </summary>
            public string Uri { get; set; }
        }
    }
}