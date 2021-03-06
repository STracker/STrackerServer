﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserPrivateView.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The user private view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The user private view.
    /// </summary>
    public class UserPrivateView
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the picture url.
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// Gets or sets the subscription list.
        /// </summary>
        public List<Subscription> SubscriptionList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is admin.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets the new episodes.
        /// </summary>
        public ICollection<TvShowCalendar> NewEpisodes { get; set; }
    }
}