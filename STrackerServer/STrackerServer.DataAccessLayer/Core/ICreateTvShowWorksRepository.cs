﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICreateTvShowWorksRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of  repositories for television show works.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The Create television shows works repository interface.
    /// </summary>
    public interface ICreateTvShowWorksRepository : IRepository<CreateTvShowWork, string>
    {
        /// <summary>
        /// The get id.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string GetId(string name);
    }
}
