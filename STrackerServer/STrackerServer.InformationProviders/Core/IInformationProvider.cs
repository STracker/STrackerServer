// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInformationProvider.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface for information provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.InformationProviders.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The Information provider interface.
    /// </summary>
    public interface IInformationProvider
    {
        /// <summary>
        /// Get a television show object with information filled.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        TvShow GetAllTvShowInformation(string imdbId);
    }
}
