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
    public class PermissionManager : IPermissionManager<int>
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