﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of television shows repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.TvShowsRepositories
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Television shows repository interface.
    /// </summary>
    public interface ITvShowsRepository : IRepository<TvShow, string>
    {
        /// <summary>
        /// The read all by genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        IEnumerable<TvShow.TvShowSynopsis> ReadAllByGenre(string genre);

        /// <summary>
        /// The read by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="STrackerServer.DataAccessLayer.DomainEntities.TvShow"/>.
        /// </returns>
        TvShow ReadByName(string name);

        /// <summary>
        /// The add season synopsis.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="season">
        /// The season.
        /// </param>
        void AddSeasonSynopsis(string tvshowId, Season.SeasonSynopsis season);

        /// <summary>
        /// The remove season synopsis.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="season">
        /// The season.
        /// </param>
        void RemoveSeasonSynopsis(string tvshowId, Season.SeasonSynopsis season);
    }
}
