// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscribeFormValues.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the SubscribeFormValues type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The subscribe form values.
    /// </summary>
    public class SubscribeFormValues
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        [Required]
        public string tvshowId { get; set; }

        /// <summary>
        /// Gets or sets the redirect url.
        /// </summary>
        public string redirectUrl { get; set; }
    }
}