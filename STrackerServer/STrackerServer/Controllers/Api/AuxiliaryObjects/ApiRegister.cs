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
    /// <summary>
    /// The API register.
    /// </summary>
    public class ApiRegister
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the photo.
        /// </summary>
        public string Photo { get; set; }
    }
}