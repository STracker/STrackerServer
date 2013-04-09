// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Episode.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Models
{
    using System;
    using System.Collections.Generic;

    using STrackerServerDatabase.Core;

    /// <summary>
    /// The episode domain entity.
    /// </summary>
    public class Episode : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Episode"/> class.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonnumber">
        /// The season number.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        public Episode(string tvshowId, int seasonnumber, int number)
        {
            this.Id = string.Format("{0}_{1}_{2}", tvshowId, seasonnumber, number);
            this.TvShowId = tvshowId;
            this.SeasonNumber = seasonnumber;
            this.Number = number;
            this.Directors = new List<Person>();
            this.Artworks = new List<Artwork>();
            this.GuestActors = new List<Actor>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Episode"/> class.
        /// </summary>
        public Episode()
        {    
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets the television show id.
        /// </summary>
        public string TvShowId { get; private set; }

        /// <summary>
        /// Gets the season number.
        /// </summary>
        public int SeasonNumber { get; private set; }

        /// <summary>
        /// Gets the episode number.
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the directors.
        /// </summary>
        public List<Person> Directors { get; set; }

        /// <summary>
        /// Gets or sets the artworks.
        /// </summary>
        public List<Artwork> Artworks { get; set; }

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
            return new EpisodeSynopsis { Number = this.Number, Name = this.Name };
        }

        /// <summary>
        /// The episode synopsis.
        /// </summary>
        public class EpisodeSynopsis
        {
            /// <summary>
            /// Gets or sets the number.
            /// </summary>
            public int Number { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }
        }
    }
}