// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiAddRating.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  This object encapsulates the necessary values for add a new 
//  rating.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AuxiliaryObjects
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The rating.
    /// </summary>
    public class ApiAddRating
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user rating.
        /// </summary>
        /// Is the type string because web api validation don't
        /// validate value types.
        [Required]
        public string UserRating { get; set; }
    }
}