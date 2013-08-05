// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INewestEpisodesRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of newest episodes repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.EpisodesRepositories
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The newest episodes repository.
    /// </summary>
    public interface INewestEpisodesRepository : IRepository<NewTvShowEpisodes, string>
    {
        /// <summary>
        /// The try add newest episode.
        /// </summary>
        /// <param name="synopsis">
        /// The synopsis.
        /// </param>
        /// <param name="tvshowSynopsis">
        /// The television show Synopsis.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool TryAddNewestEpisode(Episode.EpisodeSynopsis synopsis, TvShow.TvShowSynopsis tvshowSynopsis);

        /// <summary>
        /// The remove episode.
        /// </summary>
        /// <param name="synopsis">
        /// The synopsis.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveEpisode(Episode.EpisodeSynopsis synopsis);

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see>
        ///       <cref>ICollection</cref>
        ///     </see> .
        /// </returns>
        ICollection<NewTvShowEpisodes> GetAll();  

        /// <summary>
        /// The Delete old episodes.
        /// </summary>
        void DeleteOldEpisodes();
    }
}