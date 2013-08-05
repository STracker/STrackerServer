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
        /// Initializes a new instance of the <see cref="RatingsTvShow"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public RatingsTvShow(string id) : base(id)
        {
        }
    }
}