// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PermissionManager.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The permission manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Permissions
{
    using System.Collections.Generic;

    /// <summary>
    /// The permission manager.
    /// </summary>
    public class PermissionManager : IPermissionManager<Permission, int>
    {
        /// <summary>
        /// The has permission.
        /// </summary>
        /// <param name="permission">
        /// The permission.
        /// </param>
        /// <param name="currentPermission">
        /// The current permission.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool HasPermission(Permission permission, int currentPermission)
        {
            return this.GetPermission(currentPermission) >= permission;
        }

        /// <summary>
        /// The has permission.
        /// </summary>
        /// <param name="permission">
        /// The permission.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public Permission GetPermission(int permission)
        {
            return (Permission)permission;
        }

        /// <summary>
        /// The get permissions.
        /// </summary>
        /// <returns>
        /// The <see>
        ///       <cref>IDictionary</cref>
        ///     </see> .
        /// </returns>
        public IDictionary<Permission, int> GetPermissions()
        {
            return new Dictionary<Permission, int>
                {
                    { Permission.Default, (int)Permission.Default },
                    { Permission.Moderator, (int)Permission.Moderator },
                    { Permission.Admin, (int)Permission.Admin }
                };
        }
    }
}