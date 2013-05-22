// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFriendRequestRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The FriendRequestRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The FriendRequestRepository interface.
    /// </summary>
    public interface IFriendRequestRepository : IRepository<FriendRequest, string>
    {
        /// <summary>
        /// The read all.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        List<FriendRequest> ReadAllTo(string userId);

        /// <summary>
        /// The read all from.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        List<FriendRequest> ReadAllFrom(string userId);
    }
}
