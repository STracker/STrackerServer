// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublicFriendsView.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the PublicFriendsView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.Collections.Generic;

    /// <summary>
    /// The public friends view.
    /// </summary>
    public class PublicFriendsView
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
        public List<DataAccessLayer.DomainEntities.User.UserSynopsis> List { get; set; }
    }
}