﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodesRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of episodes repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.EpisodesRepositories
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes repository interface.
    /// </summary>
    public interface IEpisodesRepository : IRepository<Episode, Episode.EpisodeId>
    {
        /// <summary>
        /// Create several episodes.
        /// </summary>
        /// <param name="episodes">
        /// The episodes.
        /// </param>
        void CreateAll(ICollection<Episode> episodes);
    }
}