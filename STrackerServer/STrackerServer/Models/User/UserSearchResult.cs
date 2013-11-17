// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserSearchResult.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The user search result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.Collections.Generic;

    /// <summary>
    /// The user search result.
    /// </summary>
    public class UserSearchResult
    {
        /// <summary>
        /// Gets or sets the search value.
        /// </summary>
        public string SearchValue { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        public ICollection<DataAccessLayer.DomainEntities.User.UserSynopsis> Result { get; set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether has more users.
        /// </summary>
        public bool HasMoreUsers { get; set; }
    }
}