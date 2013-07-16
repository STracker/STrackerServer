// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserPublicView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  The user view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The user view.
    /// </summary>
    public class UserPublicView
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
        /// Gets or sets a value indicating whether is friend.
        /// </summary>
        public bool IsFriend { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is admin.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether admin mode.
        /// </summary>
        public bool AdminMode { get; set; }
    }
}