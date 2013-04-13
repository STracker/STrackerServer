// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ISeasonsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Facades
{
    using System;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Seasons operations.
    /// </summary>
    public class SeasonsOperations : BaseCrudOperations<Season, Tuple<string, int>>, ISeasonsOperations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public SeasonsOperations(ISeasonsRepository repository)
            : base(repository)
        {
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
    }
}
