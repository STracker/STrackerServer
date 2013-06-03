// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The Television Show comments Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The Television Show BaseComments Repository interface.
    /// </summary>
    public interface ITvShowCommentsRepository : ICommentsRepository<TvShowComments, string>
    {
    }
}