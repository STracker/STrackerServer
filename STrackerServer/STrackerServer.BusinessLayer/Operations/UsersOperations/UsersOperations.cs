﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.UsersOperations
{
    using System.Linq;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.Core.UsersRepositories;
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
        public void VerifyAndSave(User user)
        {
            var domainUser = this.Repository.Read(user.Key);

            if (domainUser == null)
            {
                this.Create(user);
                return;
            }

            if (user.Equals(domainUser))
            {
                return;
            }

            // For not lose the friends its necessary to "pass" them to new object user for update.
            user.Friends = domainUser.Friends;
            this.Update(user);
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
            var user = this.Repository.Read(userId);
            var tvshow = this.tvshowsRepository.Read(tvshowId);
            if (tvshow == null)
            {
                return false;
            }

            return !user.SubscriptionList.Any(sub => sub.Id.Equals(tvshowId)) && ((IUsersRepository)this.Repository).AddSubscription(user, tvshow.GetSynopsis());
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
            var user = this.Repository.Read(userId);
            var tvshow = this.tvshowsRepository.Read(tvshowId);
            if (tvshow == null)
            {
                return false;
            }

            return user.SubscriptionList.Any(sub => sub.Id.Equals(tvshowId)) && ((IUsersRepository)this.Repository).RemoveSubscription(user, tvshow.GetSynopsis());
        }

        /// <summary>
        /// The invite.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Invite(string from, string to)
        {
            if (from.Equals(to))
            {
                return false;
            }

            var userFrom = this.Repository.Read(from);
            var userTo = this.Repository.Read(to);
            
            if (userFrom.Friends.Exists(synopsis => synopsis.Id.Equals(to)) || userTo.FriendRequests.Exists(synopsis => synopsis.Id.Equals(from)))
            {
                return false;
            }
            
            return ((IUsersRepository)this.Repository).Invite(userFrom, userTo);
        }

        /// <summary>
        /// The accept invite.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AcceptInvite(string from, string to)
        {
            var userFrom = this.Repository.Read(from);
            var userTo = this.Repository.Read(to);

            return userTo.FriendRequests.Exists(synopsis => synopsis.Id.Equals(@from)) && ((IUsersRepository)this.Repository).AcceptInvite(userFrom, userTo);
        }

        /// <summary>
        /// The reject invite.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RejectInvite(string from, string to)
        {
            var userFrom = this.Repository.Read(from);
            var userTo = this.Repository.Read(to);

            return userTo.FriendRequests.Exists(synopsis => synopsis.Id.Equals(@from)) && ((IUsersRepository)this.Repository).RejectInvite(userFrom, userTo);
        }
    }
}