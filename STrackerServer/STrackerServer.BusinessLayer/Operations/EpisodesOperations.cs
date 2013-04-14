// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes operations.
    /// </summary>
    public class EpisodesOperations : BaseCrudOperations<Episode, Tuple<string, int, int>>, IEpisodesOperations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public EpisodesOperations(IRepository<Episode, Tuple<string, int, int>> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Create one episode.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(Episode entity)
        {
            // TODO validate fields, like tvshowid and season number verify if exists.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update one episode.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(Episode entity)
        {
            throw new NotImplementedException();
        }
    }
}
