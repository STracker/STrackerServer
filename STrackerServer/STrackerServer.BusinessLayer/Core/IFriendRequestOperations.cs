// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFriendRequestOperations.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The FriendRequestOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The FriendRequestOperations interface.
    /// </summary>
    public interface IFriendRequestOperations
    {
        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Create(FriendRequest request);

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <returns>
        /// The <see cref="FriendRequest"/>.
        /// </returns>
        FriendRequest Read(string from, string to);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Delete(string id, string userId);

        /// <summary>
        /// The accept.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Accept(string id, string userId);

        /// <summary>
        /// The reject.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Reject(string id, string userId);

        /// <summary>
        /// The get requests.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        List<FriendRequest> GetRequests(string userId);

        /// <summary>
        /// The refresh requests.
        /// </summary>
        /// <param name="userId">
        /// The user Id.
        /// </param>
        void Refresh(string userId);
    }
}
