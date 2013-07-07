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
        /// The add subscription.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddSubscription(User user, TvShow.TvShowSynopsis tvshow);

        /// <summary>
        /// The remove subscription.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveSubscription(User user, TvShow.TvShowSynopsis tvshow);

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
        bool Invite(User from, User to);

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
        bool AcceptInvite(User from, User to);

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
        bool RejectInvite(User from, User to);

        /// <summary>
        /// The send suggestion.
        /// </summary>
        /// <param name="userTo">
        /// The user To.
        /// </param>
        /// <param name="suggestion">
        /// The suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool SendSuggestion(User userTo, Suggestion suggestion);

        /// <summary>
        /// The remove suggestion.
        /// </summary>
        /// <param name="userFrom">
        /// The user from.
        /// </param>
        /// <param name="suggestion">
        /// The suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveSuggestion(User userFrom, Suggestion suggestion);

        /// <summary>
        /// The get suggestions.
        /// </summary>
        /// <param name="userFrom">
        /// The user from.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        List<Suggestion> GetSuggestions(User userFrom);

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
        List<User> FindByName(string name);

        /// <summary>
        /// The remove friend.
        /// </summary>
        /// <param name="userModel">
        /// The user model.
        /// </param>
        /// <param name="userFriend">
        /// The user friend.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveFriend(User userModel, User userFriend);

        /// <summary>
        /// The remove television show suggestions.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveTvShowSuggestions(User user, TvShow tvshow);
    }
}
