// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPermissionManager.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the IPermissionManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Permissions
{
    /// <summary>
    /// The PermissionManager interface.
    /// </summary>
    /// <typeparam name="T">
    /// Permission Type
    /// </typeparam>
    public interface IPermissionManager<T>
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