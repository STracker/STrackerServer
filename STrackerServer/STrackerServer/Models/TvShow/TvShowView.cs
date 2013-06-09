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
    using System.Linq;

    using DataAccessLayer.DomainEntities;

    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

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
            this.Artwork = tvshow.Poster;
            this.Actors = tvshow.Actors;
            this.Runtime = tvshow.Runtime;
            this.AirDay = tvshow.AirDay;
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
        public List<Genre.GenreSynopsis> Genres { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        public double Rating { get; set; }

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

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public TvShowOptions Options { get; set; }

        /// <summary>
        /// The television show options.
        /// </summary>
        public class TvShowOptions
        {
            /// <summary>
            /// Prevents a default instance of the <see cref="TvShowOptions"/> class from being created. 
            /// </summary>
            private TvShowOptions()
            {
            }

            /// <summary>
            /// Gets or sets the television show id.
            /// </summary>
            public string TvShowId { get; set; }

            /// <summary>
            /// Gets or sets the television show poster.
            /// </summary>
            public string Poster { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether is subscribed.
            /// </summary>
            public bool IsSubscribed { get; set; }

            /// <summary>
            /// Gets or sets the unsubscribe redirect url.
            /// </summary>
            public string RedirectUrl { get; set; }

            /// <summary>
            /// The create.
            /// </summary>
            /// <param name="tvshow">
            /// The television show.
            /// </param>
            /// <param name="user">
            /// The user.
            /// </param>
            /// <param name="redirectUrl">
            /// The redirect url.
            /// </param>
            /// <returns>
            /// The <see cref="TvShowOptions"/>.
            /// </returns>
            public static TvShowOptions Create(TvShow tvshow, User user, string redirectUrl)
            {
                var isSubscribed = false;

                if (user != null)
                {
                    isSubscribed = user.SubscriptionList.Any(synopsis => synopsis.Id.Equals(tvshow.TvShowId));
                }

                return new TvShowOptions
                {
                    TvShowId = tvshow.TvShowId,
                    Poster = tvshow.Poster.ImageUrl,
                    IsSubscribed = isSubscribed,
                    RedirectUrl = redirectUrl
                };
            }
        }
    }
}