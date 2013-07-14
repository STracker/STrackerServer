// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPermissionProvider.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the IPermissionProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Permissions
{
    /// <summary>
    /// The PermissionProvider interface.
    /// </summary>
    /// <typeparam name="T">
    /// Permission Type
    /// </typeparam>
    public interface IPermissionProvider<T>
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
        bool HasPermission(T permission, T currentPermission);
    }
}