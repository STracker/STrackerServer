// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FriendRequest.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of friend request domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// The friend request.
    /// </summary>
    public class FriendRequest : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the from.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the to.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it was accepted.
        /// </summary>
        public bool Accepted { get; set; }
    }
}
