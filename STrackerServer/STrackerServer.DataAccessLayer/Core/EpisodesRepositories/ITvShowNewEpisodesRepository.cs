// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowNewEpisodesRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of television shows new episodes repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.EpisodesRepositories
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Television show new episodes interface.
    /// The second generic parameter is of the type string, because the identifier is the 
    /// television show id.
    /// </summary>
    public interface ITvShowNewEpisodesRepository : IRepository<NewTvShowEpisodes, string>
    {
        /// <summary>
        /// Delete old episodes, i.e., the episodes with old dates.
        /// </summary>
        void DeleteOldEpisodes();

        /// <summary>
        /// Add new episode. Only add if the date is not more old that the current date.
        /// </summary>
        /// <param name="id">
        /// The id of television show.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddEpisode(string id, Episode.EpisodeSynopsis episode);

        /// <summary>
        /// Remove episode.
        /// </summary>
        /// <param name="id">
        /// The id of television show.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveEpisode(string id, Episode.EpisodeSynopsis episode);
    }
}