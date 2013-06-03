// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodeCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The Episode comments Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using System;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The Episode BaseComments Repository interface.
    /// </summary>
    public interface IEpisodeCommentsRepository : ICommentsRepository<EpisodeComments, Tuple<string, int, int>>
    {
    }
}