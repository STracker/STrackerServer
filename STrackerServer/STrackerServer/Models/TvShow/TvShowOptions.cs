// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowOptions.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show options.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using System.Linq;

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
        public static TvShowOptions Create(DataAccessLayer.DomainEntities.TvShow tvshow, DataAccessLayer.DomainEntities.User user, string redirectUrl)
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