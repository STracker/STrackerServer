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
        public bool HasPermission(Permission permission, Permission currentPermission)
        {
            return currentPermission >= permission;
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
    }
}