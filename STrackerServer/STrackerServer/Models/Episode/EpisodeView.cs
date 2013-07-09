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

    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The episode view.
    /// </summary>
    public class EpisodeView
    {
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
        public double Rating { get; set; }

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
    }
}