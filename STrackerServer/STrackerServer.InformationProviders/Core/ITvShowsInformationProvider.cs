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
        /// Get a television show object with information filled.
        /// </summary>
        /// <param name="tvshowId">
        /// The IMDB id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        TvShow GetTvShowInformation(string tvshowId);

        /// <summary>
        /// Get episodes information.
        /// </summary>
        /// <param name="tvshowId">
        /// The IMDB id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        IEnumerable<Episode> GetEpisodesInformation(string tvshowId);

        /// <summary>
        /// Verify if the television show exists.
        /// </summary>
        /// <param name="tvshowId">
        /// The IMDB id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool VerifyIfExists(string tvshowId);
    }
}