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

            var userFrom = this.usersRepository.Read(request.From);
            var userTo = this.usersRepository.Read(request.To);

            if (userFrom == null || userTo == null)
            {
                return false;
            }

            if (userFrom.Friends.Exists(synopsis => synopsis.Id.Equals(request.To)) || userTo.Friends.Exists(synopsis => synopsis.Id.Equals(request.From)))
            {
                return false;
            }

            request.Key = request.From + request.To;
            return this.repository.Create(request);
        }

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
        public FriendRequest Read(string from, string to)
        {
            return this.repository.Read(from + to);
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

            var request = this.repository.Read(id);

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
            var user = this.usersRepository.Read(userId);
            var request = this.repository.Read(id);

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
            var request = this.repository.Read(id);

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
            var returnList = new List<FriendRequest>();
            this.repository.ReadAllTo(userId).ForEach(request =>
            {
                 if (!request.Accepted)
                 {
                     returnList.Add(request);
                 }
            });
            return returnList;
        }

        /// <summary>
        /// The refresh requests.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        public void Refresh(string userId)
        {
            var user = this.usersRepository.Read(userId);

            var myRequests = this.repository.ReadAllFrom(userId);

            foreach (var request in myRequests)
            {
                if (request.Accepted)
                {
                    if (!user.Friends.Exists(synopsis => synopsis.Id.Equals(request.To)))
                    {
                        this.usersRepository.AddFriend(this.usersRepository.Read(request.From), this.usersRepository.Read(request.To));
                    }

                    this.repository.Delete(request.Key);
                }
                else
                {
                    if (user.Friends.Exists(synopsis => synopsis.Id.Equals(request.To)))
                    {
                        this.repository.Delete(request.Key);
                    }  
                }  
            }
        }
    }
}
