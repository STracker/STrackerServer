// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Requests.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The friend request view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.Collections.Generic;

    /// <summary>
    /// The friend request view.
    /// </summary>
    public class Requests
    {
        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        public List<FriendRequest> List { get; set; }

        /// <summary>
        /// The friend request.
        /// </summary>
        public class FriendRequest
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
            /// Gets or sets the picture.
            /// </summary>
            public string Picture { get; set; }
        }
    }
}