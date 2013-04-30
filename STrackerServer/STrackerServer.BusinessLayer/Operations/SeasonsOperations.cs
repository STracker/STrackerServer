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
        /// Try get one season.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="Season"/>.
        /// </returns>
        public Season TryRead(Tuple<string, int> id, out OperationResultState state)
        {
            this.tvshowsOperations.TryRead(id.Item1, out state);
            return state == OperationResultState.Completed ? this.Repository.Read(id) : null;
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
            return ((ISeasonsRepository)this.Repository).GetAllFromOneTvShow(tvshowId);
        }
    }
}