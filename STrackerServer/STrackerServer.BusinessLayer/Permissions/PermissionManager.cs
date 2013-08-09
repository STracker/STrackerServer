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
    public class PermissionManager : IPermissionManager<Permissions, int>
    {
        /// <summary>
        /// The has permission.
        /// </summary>
        /// <param name="permissions">
        /// The permission.
        /// </param>
        /// <param name="currentPermission">
        /// The current permission.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool HasPermission(Permissions permissions, int currentPermission)
        {
            return this.GetPermission(currentPermission) >= permissions;
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
        public Permissions GetPermission(int permission)
        {
            return (Permissions)permission;
        }

        /// <summary>
        /// The get permissions.
        /// </summary>
        /// <returns>
        /// The <see>
        ///       <cref>IDictionary</cref>
        ///     </see> .
        /// </returns>
        public IDictionary<Permissions, int> GetPermissions()
        {
            return new Dictionary<Permissions, int>
                {
                    { Permissions.Default, (int)Permissions.Default },
                    { Permissions.Moderator, (int)Permissions.Moderator },
                    { Permissions.Admin, (int)Permissions.Admin }
                };
        }
    }
}