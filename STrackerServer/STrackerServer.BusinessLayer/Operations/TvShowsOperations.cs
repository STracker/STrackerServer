// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsOperations interface.
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
    /// Television shows operations.
    /// </summary>
    public class TvShowsOperations : BaseCrudOperations<TvShow, string>, ITvShowsOperations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public TvShowsOperations(ITvShowsRepository repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Create one television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(TvShow entity)
        {
            // TODO validate fields!
            return this.Repository.Create(entity);
        }

        /// <summary>
        /// Update one television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(TvShow entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get television shows by genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public List<TvShow> ReadAllByGenre(Genre genre)
        {
            throw new NotImplementedException();
        }
    }
}
