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

    public class SuggestionView
    {
        /// <summary>
        /// Gets or sets the friends.
        /// </summary>
        public List<DataAccessLayer.DomainEntities.User.UserSynopsis> Friends { get; set; }

        public string TvShowId { get; set; }

        //Options
    }
}