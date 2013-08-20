// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUsersOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over users.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core.UsersOperations
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// Users operations interface.
    /// </summary>
    public interface IUsersOperations : ICrudOperations<User, string>
    {
        /// <summary>
        /// Verify if the user exists, if not create one. Also verify if the properties of the user have changed.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        void VerifyAndSave(User user);

        /// <summary>
        /// Get all users that have the same name of the name passed in parameters.
        /// </summary>
        /// <param name="name">
        /// The name for search.
        /// </param>
        /// <param name="range">
        /// The range.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        ICollection<User.UserSynopsis> ReadByName(string name, Range range = null);

        /// <summary>
        /// Add one subscription to user's subscription list.
        /// </summary>
        /// <param name="id">
        /// The id of the user.
        /// </param>
        /// <param name="tvshowId">
        /// The television show Id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddSubscription(string id, string tvshowId);

        /// <summary>
        /// Remove one subscription from user's subscription list.
        /// </summary>
        /// <param name="id">
        /// The id of the user.
        /// </param>
        /// <param name="tvshowId">
        /// The television show Id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveSubscription(string id, string tvshowId);

        /// <summary>
        /// Invite one user to make part of the friends list.
        /// </summary>
        /// <param name="userFromId">
        /// The user, that invites, id.
        /// </param>
        /// <param name="userToId">
        /// The user, that receive the invite, id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool InviteFriend(string userFromId, string userToId);

        /// <summary>
        /// Accept invitation from one user to be part of the friends list.
        /// </summary>
        /// <param name="userFromId">
        /// The user, that invites, id.
        /// </param>
        /// <param name="userToId">
        /// The user, that receive the invite, id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AcceptInvite(string userFromId, string userToId);

        /// <summary>
        /// Reject invitation from one user.
        /// </summary>
        /// <param name="userFromId">
        /// The user, that invites, id.
        /// </param>
        /// <param name="userToId">
        /// The user, that receive the invite, id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RejectInvite(string userFromId, string userToId);

        /// <summary>
        /// Remove one friend from user's friends list.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="friendId">
        /// The friend id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveFriend(string id, string friendId);

        /// <summary>
        /// Send one television show suggestion to one user.
        /// </summary>
        /// <param name="userFrom">
        /// The user that sends the suggestion.
        /// </param>
        /// <param name="userTo">
        /// The user that receives the suggestion.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        ///  </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool SendSuggestion(string userFrom, string userTo, string tvshowId);

        /// <summary>
        /// Remove all suggestions of one television show.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="tvshowId">
        /// The television show Id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveTvShowSuggestions(string id, string tvshowId);

        /// <summary>
        /// Add one new watched episode to episodes watched list in the television show 
        /// subscription.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="episodeId">
        /// The episode id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddWatchedEpisode(string id, Episode.EpisodeId episodeId);

        /// <summary>
        /// Remove one new watched episode from episodes watched list in the television show 
        /// subscription.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="episodeId">
        /// The episode id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveWatchedEpisode(string id, Episode.EpisodeId episodeId);

        /// <summary>
        /// Get the new episodes from user's subscription list.
        /// If the date is null, return all new episodes from all television shows in subscription list.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        ICollection<TvShowCalendar> GetUserNewEpisodes(string userId, string date);

        /// <summary>
        /// Change user permission.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="permission">
        /// The permission value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool SetUserPermission(string userId, int permission);

        /// <summary>
        /// Get the user's calendar.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        byte[] GetCalendar(string userId);
    }
}