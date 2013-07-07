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
        /// The add subscription.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="tvshowId">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddSubscription(string userId, string tvshowId);

        /// <summary>
        /// The remove subscription.
        /// </summary>
        /// <param name="userId">
        /// The name.
        /// </param>
        /// <param name="tvshowId">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveSubscription(string userId, string tvshowId);

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
        bool Invite(string from, string to);

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
        bool AcceptInvite(string from, string to);

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
        bool RejectInvite(string from, string to);

        /// <summary>
        /// The send suggestion.
        /// </summary>
        /// <param name="userTo">
        /// The user to.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="suggestion">
        /// The suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool SendSuggestion(string userTo, string tvshowId, Suggestion suggestion);

        /// <summary>
        /// The remove suggestion.
        /// </summary>
        /// <param name="userTo">
        /// The user to.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="suggestion">
        /// The suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveSuggestion(string userTo, string tvshowId, Suggestion suggestion);

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
        List<Suggestion> GetSuggestions(string userFrom, string tvshowId);

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
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="friendId">
        /// The friend id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveFriend(string userId, string friendId);

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
        bool RemoveTvShowSuggestions(string userId, string tvshowId);
    }
}
