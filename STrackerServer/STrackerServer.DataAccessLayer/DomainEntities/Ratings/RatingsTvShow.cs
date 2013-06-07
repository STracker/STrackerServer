// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RatingsTvShow.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of television show ratings domain entity. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.Ratings
{
    /// <summary>
    /// The television show ratings.
    /// </summary>
    public class RatingsTvShow : RatingsBase<string>
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }
    }
}