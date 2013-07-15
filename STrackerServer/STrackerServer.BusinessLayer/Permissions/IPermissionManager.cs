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
    /// <typeparam name="PT">
    /// Permission Type
    ///  </typeparam>
    /// <typeparam name="P">
    /// Permission return type
    /// </typeparam>
    public interface IPermissionManager<PT, P>
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
        bool HasPermission(PT permission, PT currentPermission);

        /// <summary>
        /// The has permission.
        /// </summary>
        /// <param name="permission">
        /// The permission.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        PT GetPermission(P permission);
    }
}