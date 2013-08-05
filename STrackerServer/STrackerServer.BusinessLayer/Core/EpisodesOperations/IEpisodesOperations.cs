// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over episodes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core.EpisodesOperations
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes operations interface.
    /// </summary>
    public interface IEpisodesOperations : ICrudOperations<Episode, Episode.EpisodeKey>
    {
        /// <summary>
        /// The get all from one season.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        ICollection<Episode.EpisodeSynopsis> GetAllFromOneSeason(Season.SeasonKey id);
    }
}