// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.UsersOperations
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.Core.UsersRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

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
        /// The episodes operations.
        /// </summary>
        private readonly IEpisodesOperations episodesOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="tvshowsRepository">
        /// The television shows repository.
        /// </param>
        /// <param name="episodesOperations">
        /// The episodes operations.
        /// </param>
        public UsersOperations(IUsersRepository repository, ITvShowsRepository tvshowsRepository, IEpisodesOperations episodesOperations)
            : base(repository)
        {
            this.tvshowsRepository = tvshowsRepository;
            this.episodesOperations = episodesOperations;
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

            domainUser.Email = user.Email;

            this.Update(domainUser);
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
            User user;
            TvShow tvshow;
            
            if (!this.VerifyUserAndTvshow(userId, tvshowId, out user, out tvshow))
            {
                return false;
            }

            if (user.SubscriptionList.Exists(sub => sub.TvShow.Id.Equals(tvshowId)))
            {
                return true;
            }

            var subscription = new Subscription { TvShow = tvshow.GetSynopsis() };
            return ((IUsersRepository)this.Repository).AddSubscription(user, subscription);
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
            User user;
            TvShow tvshow;

            if (!this.VerifyUserAndTvshow(userId, tvshowId, out user, out tvshow))
            {
                return false;
            }

            var subscription = user.SubscriptionList.Find(sub => sub.TvShow.Id.Equals(tvshowId));
            return subscription == null || ((IUsersRepository)this.Repository).RemoveSubscription(user, subscription);
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
            
            if (userFrom == null || userTo == null)
            {
                return false;
            }

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

            if (userFrom == null || userTo == null)
            {
                return false;
            }

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

            if (userFrom == null || userTo == null)
            {
                return false;
            }

            return userTo.FriendRequests.Exists(synopsis => synopsis.Id.Equals(from)) && ((IUsersRepository)this.Repository).RejectInvite(userFrom, userTo);
        }

        /// <summary>
        /// The send suggestion.
        /// </summary>
        /// <param name="userFrom">
        /// The user From.
        /// </param>
        /// <param name="userTo">
        /// The user to.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SendSuggestion(string userFrom, string userTo, string tvshowId)
        {
            var userFromModel = this.Repository.Read(userFrom);
            var userToModel = this.Repository.Read(userTo);
            var tvshow = this.tvshowsRepository.Read(tvshowId);

            if (userFromModel == null || userToModel == null || tvshow == null)
            {
                return false;
            }

            return userToModel.Friends.Exists(synopsis => synopsis.Id.Equals(userFromModel.Key)) && ((IUsersRepository)this.Repository).SendSuggestion(userToModel, new Suggestion { TvShow = tvshow.GetSynopsis(), User = userFromModel.GetSynopsis() });
        }

        /// <summary>
        /// The remove suggestion.
        /// </summary>
        /// <param name="userFrom">
        /// The user from.
        /// </param>
        /// <param name="userTo">
        /// The user To.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveSuggestion(string userFrom, string userTo, string tvshowId)
        {
            var userFromModel = this.Repository.Read(userFrom);
            var userToModel = this.Repository.Read(userTo);
            var tvshow = this.tvshowsRepository.Read(tvshowId);

            if (userFromModel == null || userToModel == null || tvshow == null)
            {
                return false;
            }

            return ((IUsersRepository)this.Repository).RemoveSuggestion(userToModel, new Suggestion { TvShow = tvshow.GetSynopsis(), User = userFromModel.GetSynopsis() });
        }

        /// <summary>
        /// The get suggestions.
        /// </summary>
        /// <param name="userFrom">
        /// The user from.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public List<Suggestion> GetSuggestions(string userFrom, string tvshowId)
        {
            TvShow tvshow;
            User user;

            return !this.VerifyUserAndTvshow(userFrom, tvshowId, out user, out tvshow) ? null : ((IUsersRepository)this.Repository).GetSuggestions(user);
        }

        /// <summary>
        /// The find by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public List<User.UserSynopsis> FindByName(string name)
        {
            return ((IUsersRepository)this.Repository).FindByName(name).ConvertAll(input => input.GetSynopsis());
        }

        /// <summary>
        /// The remove friend.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="friendId">
        /// The friend id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveFriend(string userId, string friendId)
        {
            var userModel = this.Repository.Read(userId);
            var userFriend = this.Repository.Read(friendId);

            if (userModel == null || userFriend == null)
            {
                return false;
            }

            if (!userModel.Friends.Exists(synopsis => synopsis.Id.Equals(friendId)))
            {
                return false;
            }

            return ((IUsersRepository)this.Repository).RemoveFriend(userModel, userFriend);
        }

        /// <summary>
        /// The remove television show suggestions.
        /// </summary>
        /// <param name="userId">
        /// The user Id.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveTvShowSuggestions(string userId, string tvshowId)
        {
            TvShow tvshow;
            User user;

            if (!this.VerifyUserAndTvshow(userId, tvshowId, out user, out tvshow))
            {
                return false;
            }

            if (!user.Suggestions.Exists(suggestion => suggestion.TvShow.Id.Equals(tvshowId)))
            {
                return true;
            }

            return ((IUsersRepository)this.Repository).RemoveTvShowSuggestions(user, tvshow);
        }

        /// <summary>
        /// The add watched episode.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="tvshowId">
        /// The television  show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="episodeNumber">
        /// The episode number.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddWatchedEpisode(string userId, string tvshowId, int seasonNumber, int episodeNumber)
        {
            var user = this.Repository.Read(userId);
            var episode = this.episodesOperations.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));

            if (user == null || episode == null)
            {
                return false;
            }

            var episodeDate = DateTime.ParseExact(episode.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var systemDate = DateTime.Now;

            if (DateTime.Compare(episodeDate, systemDate) > 0)
            {
                return false;
            }

            var subscription = user.SubscriptionList.Find(sub => sub.TvShow.Id.Equals(episode.TvShowId));

            if (subscription == null)
            {
                return false;
            }

            if (subscription.EpisodesWatched.Exists(synopsis => synopsis.Equals(episode.GetSynopsis())))
            {
                return true;
            }

            return ((IUsersRepository)this.Repository).AddWatchedEpisode(user, episode.GetSynopsis());
        }

        /// <summary>
        /// The remove watched episode.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="episodeNumber">
        /// The episode number.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveWatchedEpisode(string userId, string tvshowId, int seasonNumber, int episodeNumber)
        {
            var user = this.Repository.Read(userId);
            var episode = this.episodesOperations.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));

            if (user == null || episode == null)
            {
                return false;
            }

            var subscription = user.SubscriptionList.Find(sub => sub.TvShow.Id.Equals(episode.TvShowId));

            if (subscription == null)
            {
                return false;
            }

            var sinopsys = subscription.EpisodesWatched.Find(synopsis => synopsis.Equals(episode.GetSynopsis()));

            if (sinopsys == null)
            {
                return true;
            }

            return ((IUsersRepository)this.Repository).RemoveWatchedEpisode(user, sinopsys);
        }

        /// <summary>
        /// The verify user and television show.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool VerifyUserAndTvshow(string userId, string tvshowId, out User user, out TvShow tvshow)
        {
            tvshow = this.tvshowsRepository.Read(tvshowId);
            user = this.Repository.Read(userId);

            return tvshow != null && user != null;
        }
    }
}