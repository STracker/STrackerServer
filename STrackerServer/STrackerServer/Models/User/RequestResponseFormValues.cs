// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestResponseFormValues.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The request response form values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The request response form values.
    /// </summary>
    public class RequestResponseFormValues
    {
        /// <summary>
        /// Gets or sets a value indicating whether accept.
        /// </summary>
        [Required]
        public bool Accept { get; set; }

        /// <summary>
        /// Gets or sets the user id that made the request.
        /// </summary>
        [Required]
        public string UserId { get; set; }
    }
}