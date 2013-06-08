// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowOptionsView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show options.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Partial
{
    using System.Linq;

    /// <summary>
    /// The television show options.
    /// </summary>
    public class TvShowOptionsView
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="TvShowOptionsView"/> class from being created.
        /// </summary>
        private TvShowOptionsView()
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
        /// The <see cref="TvShowOptionsView"/>.
        /// </returns>
        public static TvShowOptionsView Create(DataAccessLayer.DomainEntities.TvShow tvshow, DataAccessLayer.DomainEntities.User user, string redirectUrl)
        {
            var isSubscribed = false;

            if (user != null)
            {
                isSubscribed = user.SubscriptionList.Any(synopsis => synopsis.Id.Equals(tvshow.TvShowId));
            }

            return new TvShowOptionsView
            {
                TvShowId = tvshow.TvShowId,
                Poster = tvshow.Poster.ImageUrl,
                IsSubscribed = isSubscribed,
                RedirectUrl = redirectUrl
            };
        }
    }
}