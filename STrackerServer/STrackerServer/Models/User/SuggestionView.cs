﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuggestionView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The suggestion view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The suggestion view.
    /// </summary>
    public class SuggestionView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestionView"/> class.
        /// </summary>
        public SuggestionView()
        {
            this.Friends = new List<User.UserSynopsis>();
        }

        /// <summary>
        /// Gets or sets the television show name.
        /// </summary>
        public string TvShowName { get; set; }

        /// <summary>
        /// Gets or sets the friends.
        /// </summary>
        public IList<User.UserSynopsis> Friends { get; set; }
    }
}