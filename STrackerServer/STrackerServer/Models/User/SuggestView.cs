﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuggestView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the SuggestView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.Collections.Generic;

    /// <summary>
    /// The suggestion view.
    /// </summary>
    public class SuggestView
    {
        /// <summary>
        /// Gets or sets the friends.
        /// </summary>
        public List<SuggestFriendView> Friends { get; set; }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary>
        public object Poster { get; set; }

        /// <summary>
        /// Gets or sets the television show name.
        /// </summary>
        public object TvShowName { get; set; }
    }
}