// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InviteFormValues.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The invite form values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The invite form values.
    /// </summary>
    public class InviteFormValues
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Required]
        public string UserId { get; set; }
    }
}