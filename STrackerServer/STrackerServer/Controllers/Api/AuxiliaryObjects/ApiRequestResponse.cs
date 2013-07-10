// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiRequestResponse.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The API request response.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AuxiliaryObjects
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The API request response.
    /// </summary>
    public class ApiRequestResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether accept.
        /// </summary>
        [Required]
        public bool Accept { get; set; }
    }
}