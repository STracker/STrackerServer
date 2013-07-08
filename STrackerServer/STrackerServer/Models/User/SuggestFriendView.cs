// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuggestFriendView.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The suggest friend view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    /// <summary>
    /// The suggest friend view.
    /// </summary>
    public class SuggestFriendView
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
        /// Gets or sets a value indicating whether is subscribed.
        /// </summary>
        public bool IsSubscribed { get; set; }
    }
}