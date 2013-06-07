// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ISeasonsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Seasons operations.
    /// </summary>
    public class SeasonsOperations : BaseCrudOperations<Season, Tuple<string, int>>, ISeasonsOperations
    {
        /// <summary>
        /// Television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="tvshowsOperations">
        /// Television shows operations.
        /// </param>
        public SeasonsOperations(ISeasonsRepository repository, ITvShowsOperations tvshowsOperations)
            : base(repository)
        {
            this.tvshowsOperations = tvshowsOperations;
        }

        /// <summary>
        ///  Get one season from one television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Season"/>.
        /// </returns>
        public override Season Read(Tuple<string, int> id)
        {
            var tvshow = this.tvshowsOperations.Read(id.Item1);
            return (tvshow == null) ? null : this.Repository.Read(id);
        }

        /// <summary>
        /// Get all seasons synopsis from one television show.
        /// </summary>
        /// <param name="tvshowId">
        /// Television show id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<Season.SeasonSynopsis> GetAllFromOneTvShow(string tvshowId)
        {
            var tvshow = this.tvshowsOperations.Read(tvshowId);
            return tvshow == null ? null : ((ISeasonsRepository)this.Repository).GetAllFromOneTvShow(tvshowId);
        }
    }
}