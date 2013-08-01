// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rating.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Implementation of rating object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities
{
    /// <summary>
    /// The rating.
    /// </summary>
    public class Rating
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public User.UserSynopsis User { get; set; }

        /// <summary>
        /// Gets or sets the user rating.
        /// </summary>
        public int UserRating { get; set; }
    }
}
