// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiAddUserSubscription.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  This object encapsulates the two necessary values for add a new 
//  subscription.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AuxiliaryObjects
{
    /// <summary>
    /// The API add user subscription.
    /// </summary>
    public class ApiAddUserSubscription
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }
    }
}