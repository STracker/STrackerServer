// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of television shows repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Repositories
{
    using STrackerServerDatabase.Core;
    using STrackerServerDatabase.Models;

    /// <summary>
    /// Television shows repository interface.
    /// </summary>
    public interface ITvShowsRepository : IRepository<TvShow, string>
    {
        // TODO, add additional stuff...
    }
}
