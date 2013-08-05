// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Episode.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of episode domain entity. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// CommentsEpisode domain entity.
    /// </summary>
    public class Episode : IEntity<Episode.EpisodeKey>
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
        /// Initializes a new instance of the <see cref="Episode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Episode(EpisodeKey id) : this()
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public EpisodeKey Id { get; set; }

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
        /// Gets or sets the date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary>
        public string Poster { get; set; }

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
            var uri = string.Format("tvshows/{0}/seasons/{1}/episodes/{2}", this.Id.TvshowId, this.Id.SeasonNumber, this.Id.EpisodeNumber);

            return new EpisodeSynopsis
                {
                    Id = this.Id,
                    Name = this.Name, 
                    Date = this.Date, 
                    Uri = uri
                };
        }

        /// <summary>
        /// CommentsEpisode synopsis object.
        /// </summary>
        public class EpisodeSynopsis : ISynopsis
        {
            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            public EpisodeKey Id { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the date.
            /// </summary>
            public string Date { get; set; }

            /// <summary>
            /// Gets or sets the uri.
            /// </summary>
            public string Uri { get; set; }

            /// <summary>
            /// The equals.
            /// </summary>
            /// <param name="episode">
            /// The episode.
            /// </param>
            /// <returns>
            /// The <see cref="bool"/>.
            /// </returns>
            public bool Equals(EpisodeSynopsis episode)
            {
                return this.Id.TvshowId.Equals(episode.Id.TvshowId) &&
                       this.Id.SeasonNumber.Equals(episode.Id.SeasonNumber) &&
                       this.Id.EpisodeNumber.Equals(episode.Id.EpisodeNumber);
            }
        }

        /// <summary>
        /// The episodes key object.
        /// </summary>
        public class EpisodeKey
        {
            /// <summary>
            /// Gets or sets the television show id.
            /// </summary>
            public string TvshowId { get; set; }

            /// <summary>
            /// Gets or sets the season number.
            /// </summary>
            public int SeasonNumber { get; set; }

            /// <summary>
            /// Gets or sets the episode number.
            /// </summary>
            public int EpisodeNumber { get; set; }
        }
    }
}