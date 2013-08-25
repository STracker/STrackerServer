// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiRegister.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the ApiRegister type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AuxiliaryObjects
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The API register.
    /// </summary>
    public class ApiRegister
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required]
        public string Email { get; set; }
    }
}