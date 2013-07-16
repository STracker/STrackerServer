// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetPermissionView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The set permission view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Admin
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using STrackerServer.BusinessLayer.Permissions;

    /// <summary>
    /// The set permission view.
    /// </summary>
    public class SetPermissionView
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the picture url.
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        [Required]
        [Range(0, 1)]
        public int Permission { get; set; }

        /// <summary>
        /// Gets or sets the permission name.
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        public IDictionary<Permission, int> Permissions { get; set; }
    }
}