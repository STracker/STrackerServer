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
        /// Create one season.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(Season entity)
        {
            // TODO validate fields, like tvshowid verify if exists.
            throw new NotImplementedException();
        }

        public override Season Read(Tuple<string, int> id)
        {
            throw new NotImplementedException();
        }

        public override Season ReadAsync(Tuple<string, int> id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update one season.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(Season entity)
        {
            throw new NotImplementedException();
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
            return this.tvshowsOperations.Read(tvshowId) == null ? new List<Season.SeasonSynopsis>() : ((ISeasonsRepository)this.Repository).GetAllFromOneTvShow(tvshowId);
        }
    }
}
