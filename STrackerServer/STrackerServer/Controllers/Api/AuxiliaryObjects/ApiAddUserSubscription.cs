﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiAddUserSubscription.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  This object encapsulates the necessary value for add a new 
//  subscription.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AuxiliaryObjects
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The API add user subscription.
    /// </summary>
    public class ApiAddUserSubscription
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        [Required]
        public string TvShowId { get; set; }
    }
}