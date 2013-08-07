﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodeCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The CommentsEpisode comments Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.EpisodesRepositories
{
    using System;

    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    /// <summary>
    /// The CommentsEpisode CommentsBase Repository interface.
    /// </summary>
    public interface IEpisodeCommentsRepository : ICommentsRepository<CommentsEpisode, Tuple<string, int, int>>
    {
    }
}