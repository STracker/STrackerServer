// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUsersRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of users repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.UsersRepositories
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// Users repository interface.
    /// </summary>
    public interface IUsersRepository : IRepository<User, string>
    {
        /// <summary>
        /// Get all users that have the same name of the name passed in parameters.
        /// </summary>
        /// <param name="name">
        /// The name for search.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        ICollection<User> ReadByName(string name);

        /// <summary>
        /// Add one subscription to user's subscription list.
        /// </summary>
        /// <param name="id">
        /// The id of the user.
        /// </param>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddSubscription(string id, Subscription subscription);

        /// <summary>
        /// Remove one subscription from user's subscription list.
        /// </summary>
        /// <param name="id">
        /// The id of the user.
        /// </param>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveSubscription(string id, Subscription subscription);

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
        /// <param name="userToId">
        /// The user that receives the suggestion.
        /// </param>
        /// <param name="suggestion">
        /// The suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool SendSuggestion(string userToId, Suggestion suggestion);

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
        /// <param name="episode">
        /// The episode watched synopsis.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddWatchedEpisode(string id, Episode.EpisodeSynopsis episode);

        /// <summary>
        /// Remove one new watched episode from episodes watched list in the television show 
        /// subscription.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="episode">
        /// The episode watched synopsis.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveWatchedEpisode(string id, Episode.EpisodeSynopsis episode);
    }
}