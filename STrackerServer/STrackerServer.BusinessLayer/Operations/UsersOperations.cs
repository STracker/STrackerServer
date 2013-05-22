// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System.Linq;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The users operations.
    /// </summary>
    public class UsersOperations : BaseCrudOperations<User, string>, IUsersOperations
    {
        /// <summary>
        /// The television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="tvshowsRepository"> 
        /// The television shows repository.
        /// </param>
        public UsersOperations(IUsersRepository repository, ITvShowsRepository tvshowsRepository)
            : base(repository)
        {
            this.tvshowsRepository = tvshowsRepository;
        }

        /// <summary>
        /// Get one user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        public override User Read(string id)
        {
            return this.Repository.Read(id);
        }

        /// <summary>
        /// Verify if the user exists, if not create one. Also verify if the properties of the user have changed.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool VerifyAndSave(User user)
        {
            var domainUser = this.Repository.Read(user.Key);
            if (domainUser == null)
            {
                return this.Create(user);
            }

            if (user.Equals(domainUser))
            {
                return true;
            }

            // For not lose the friends its necessary to "pass" them to new object user for update.
            user.Friends = domainUser.Friends;
            return this.Update(user);
        }

        /// <summary>
        /// The add subscription.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddSubscription(string userId, string tvshowId)
        {
            User user = Repository.Read(userId);

            if (user == null)
            {
                return false;
            }

            TvShow tvshow = this.tvshowsRepository.Read(tvshowId);

            if (tvshow == null)
            {
                return false;
            }

            if (user.SubscriptionList.Any(sub => sub.Id.Equals(tvshowId)))
            {
                return false;
            }

            return ((IUsersRepository)Repository).AddSubscription(user, tvshow);
        }

        /// <summary>
        /// The remove subscription.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveSubscription(string userId, string tvshowId)
        {
            User user = Repository.Read(userId);

            if (user == null)
            {
                return false;
            }

            TvShow tvshow = this.tvshowsRepository.Read(tvshowId);

            if (tvshow == null)
            {
                return false;
            }

            if (!user.SubscriptionList.Any(sub => sub.Id.Equals(tvshowId)))
            {
                return false;
            }

            return ((IUsersRepository)Repository).RemoveSubscription(user, tvshow);
        }
    }
}