// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FriendRequestOperations.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the FriendRequestOperations type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System.Collections.Generic;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The friend request operations.
    /// </summary>
    public class FriendRequestOperations : IFriendRequestOperations
    {
        /// <summary>
        /// Gets the repository.
        /// </summary>
        private readonly IFriendRequestRepository repository;

        /// <summary>
        /// Gets the repository.
        /// </summary>
        private readonly IUsersRepository usersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendRequestOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="usersRepository">
        /// The users Repository.
        /// </param>
        public FriendRequestOperations(IFriendRequestRepository repository, IUsersRepository usersRepository)
        {
            this.repository = repository;
            this.usersRepository = usersRepository;
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Create(FriendRequest request)
        {
            if (request.To.Equals(request.From))
            {
                return false;
            }

            User userFrom = this.usersRepository.Read(request.From);
            User userTo = this.usersRepository.Read(request.To);

            if (userFrom == null || userTo == null)
            {
                return false;
            }

            if (userFrom.Friends.Exists(synopsis => synopsis.Id.Equals(request.To)) || userTo.Friends.Exists(synopsis => synopsis.Id.Equals(request.From)))
            {
                return false;
            }

            return this.repository.Create(request);
        }

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
        public bool Delete(string id, string userId)
        {
            if (!this.usersRepository.Exists(userId))
            {
                return false;
            }

            FriendRequest request = this.repository.Read(id);

            if (request == null || (!userId.Equals(request.From) && !userId.Equals(request.To)))
            {
                return false;
            }

            return this.repository.Delete(id);
        }

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
        public bool Accept(string id, string userId)
        {
            User user = this.usersRepository.Read(userId);
            FriendRequest request = this.repository.Read(id);

            if (request == null || !userId.Equals(request.To))
            {
                return false;
            }

            request.Accepted = true;

            if (!this.repository.Update(request))
            {
                return false;
            }

            return this.usersRepository.AddFriend(user, this.usersRepository.Read(request.From));
        }

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
        public bool Reject(string id, string userId)
        {
            FriendRequest request = this.repository.Read(id);

            if (request == null || !userId.Equals(request.To))
            {
                return false;
            }

            return this.repository.Delete(id);
        }

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
        public List<FriendRequest> GetRequests(string userId)
        {
            return !this.usersRepository.Exists(userId) ? null : this.repository.ReadAllTo(userId);
        }

        /// <summary>
        /// The refresh requests.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        public void Refresh(string userId)
        {
            List<FriendRequest> myRequests = this.repository.ReadAllFrom(userId);

            foreach (var friendRequest in myRequests)
            {
                this.usersRepository.AddFriend(this.usersRepository.Read(friendRequest.From), this.usersRepository.Read(friendRequest.To));
                this.repository.Delete(friendRequest.Key);
            }
        }
    }
}
