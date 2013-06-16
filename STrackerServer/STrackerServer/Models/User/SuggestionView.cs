// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuggestionView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the SuggestionView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.Collections.Generic;

    /// <summary>
    /// The suggestion view.
    /// </summary>
    public class SuggestionView
    {
        /// <summary>
        /// Gets or sets the friends.
        /// </summary>
        public List<DataAccessLayer.DomainEntities.User.UserSynopsis> Friends { get; set; }

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