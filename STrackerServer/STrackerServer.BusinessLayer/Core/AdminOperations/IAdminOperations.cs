// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdminOperations.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The AdminOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core.AdminOperations
{
    using STrackerServer.BusinessLayer.Permissions;

    /// <summary>
    /// The AdminOperations interface.
    /// </summary>
    public interface IAdminOperations
    {
        /// <summary>
        /// The set user permission.
        /// </summary>
        /// <param name="adminId">
        /// The admin id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="permission">
        /// The permission.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool SetUserPermission(string adminId, string userId, Permission permission);
    }
}
