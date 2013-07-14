// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PermissionProvider.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The permission provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Permissions
{
    /// <summary>
    /// The permission provider.
    /// </summary>
    public class PermissionProvider : IPermissionProvider<int>
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
        public bool HasPermission(int permission, int currentPermission)
        {
            return currentPermission >= permission;
        }
    }
}