// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowsInformationProvider.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface for information provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.InformationProviders.Core
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The Information provider interface.
    /// </summary>
    public interface ITvShowsInformationProvider
    {
        /// <summary>
        /// Verify if the television show exists.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool VerifyIfExists(string imdbId);

        /// <summary>
        /// The verify if exists by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string VerifyIfExistsByName(string name);

        /// <summary>
        /// Get a television show object with information filled.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        TvShow GetTvShowInformation(string imdbId);

        /// <summary>
        /// Get seasons information.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB Id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        IEnumerable<Season> GetSeasonsInformation(string imdbId);

        /// <summary>
        /// Get episodes information.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB Id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season Number.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        IEnumerable<Episode> GetEpisodesInformation(string imdbId, int seasonNumber);
    }
}