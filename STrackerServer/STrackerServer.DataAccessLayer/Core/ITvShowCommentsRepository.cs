// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The Television Show Comments Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The Television Show Comments Repository interface.
    /// </summary>
    public interface ITvShowCommentsRepository : IRepository<TvShowComments, string>
    {
    }
}
