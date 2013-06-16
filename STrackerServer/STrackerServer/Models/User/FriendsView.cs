// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FriendsView.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The friends view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.Collections.Generic;

    /// <summary>
    /// The friends view.
    /// </summary>
    public class FriendsView
    {
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
        public List<DataAccessLayer.DomainEntities.User.UserSynopsis> List { get; set; }
    }
}