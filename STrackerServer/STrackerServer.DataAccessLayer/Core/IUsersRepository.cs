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
        // TODO
    }
}
