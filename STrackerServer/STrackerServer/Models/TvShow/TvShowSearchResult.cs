// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowSearchResult.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show search result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The television show search result.
    /// </summary>
    public class TvShowSearchResult
    {
        /// <summary>
        /// Gets or sets the search value.
        /// </summary>
        public string SearchValue { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        public ICollection<TvShow.TvShowSynopsis> Result { get; set; }
    }
}