// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeView.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episode view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Episode
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The episode view.
    /// </summary>
    public class EpisodeView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeView"/> class.
        /// </summary>
        /// <param name="episode">
        /// The episode.
        /// </param>
        public EpisodeView(Episode episode)
        {
            this.TvShowId = episode.Id.TvShowId;
            this.SeasonNumber = episode.Id.SeasonNumber;
            this.EpisodeNumber = episode.Id.EpisodeNumber;
            this.Description = episode.Description;
            this.Name = episode.Name;
            this.GuestActors = episode.GuestActors;
            this.Directors = episode.Directors;
            this.Poster = episode.Poster;
            this.Date = episode.Date;
        }

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
        /// Gets or sets the rating.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the directors.
        /// </summary>
        public List<Person> Directors { get; set; }

        /// <summary>
        /// Gets or sets the guest actors.
        /// </summary>
        public List<Actor> GuestActors { get; set; }

        /// <summary>
        /// Gets or sets the television show name.
        /// </summary>
        public string TvShowName { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary>
        public string Poster { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the episode was watched.
        /// </summary>
        public bool Watched { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is subscribed.
        /// </summary>
        public bool IsSubscribed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether as aired.
        /// </summary>
        public bool AsAired { get; set; }

        /// <summary>
        /// Gets or sets the ratings count.
        /// </summary>
        public int RatingsCount { get; set; }

        /// <summary>
        /// Gets or sets the user rating.
        /// </summary>
        public int UserRating { get; set; }
    }
}