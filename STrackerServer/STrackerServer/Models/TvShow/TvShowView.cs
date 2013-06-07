// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Television Show Web Controller
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using System.Collections.Generic;
    using DataAccessLayer.DomainEntities;

    /// <summary>
    /// The view model of the television show.
    /// </summary>
    public class TvShowView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowView"/> class.
        /// </summary>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        public TvShowView(TvShow tvshow)
        {
            this.TvShowId = tvshow.TvShowId;
            this.Name = tvshow.Name;
            this.Description = tvshow.Description;
            this.Genres = tvshow.Genres;
            this.Artwork = tvshow.Artworks[0];
            this.Actors = tvshow.Actors;
            this.Runtime = tvshow.Runtime;
            this.AirDay = tvshow.AirDay;
            this.Creator = tvshow.Creator;
            this.SeasonSynopses = tvshow.SeasonSynopses;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user is subscribed.
        /// </summary>
        public bool IsSubscribed { get; set; }

        /// <summary>
        /// Gets or sets the television show IMDB id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the genres.
        /// </summary>
        public List<string> Genres { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the artworks.
        /// </summary>
        public Artwork Artwork { get; set; }

        /// <summary>
        /// Gets or sets the actors.
        /// </summary>
        public List<Actor> Actors { get; set; }

        /// <summary>
        /// Gets or sets the air day.
        /// </summary>
        public string AirDay { get; set; }

        /// <summary>
        /// Gets or sets the runtime.
        /// </summary>
        public int Runtime { get; set; }

        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        public Person Creator { get; set; }

        /// <summary>
        /// Gets or sets the season synopses.
        /// </summary>
        public List<Season.SeasonSynopsis> SeasonSynopses { get; set; }
    }
}