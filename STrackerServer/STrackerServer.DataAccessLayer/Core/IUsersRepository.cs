// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUsersRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of users repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

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
        bool AddSubscription(User user, TvShow tvshow);

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
        bool RemoveSubscription(User user, TvShow tvshow);
    }
}
