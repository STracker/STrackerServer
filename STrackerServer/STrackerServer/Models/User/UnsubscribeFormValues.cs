// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnsubscribeFormValues.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The unsubscribe form values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The unsubscribe form values.
    /// </summary>
    public class UnsubscribeFormValues
    {        
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        [Required]
        public string TvshowId { get; set; }

        /// <summary>
        /// Gets or sets the redirect url.
        /// </summary>
        [Required]
        public string RedirectUrl { get; set; }
    }
}