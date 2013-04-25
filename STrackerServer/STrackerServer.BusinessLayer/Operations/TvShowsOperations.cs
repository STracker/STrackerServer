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
    using STrackerServer.WorkQueue.Core;

    /// <summary>
    /// Television shows operations.
    /// </summary>
    public class TvShowsOperations : BaseCrudOperations<TvShow, string>, ITvShowsOperations
    {
        /// <summary>
        /// The work queue.
        /// </summary>
        private IWorkQueueForTvShows workQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="workQueue">
        /// The work Queue.
        /// </param>
        public TvShowsOperations(ITvShowsRepository repository, IWorkQueueForTvShows workQueue)
            : base(repository)
        {
            this.workQueue = workQueue;
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
        /// Get one television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override TvShow Read(string id)
        {
            if (this.workQueue.ExistsWork(id))
            {
                return null;
            }

            var tvshow = this.Repository.Read(id);

            if (tvshow != null)
            {
                return tvshow;
            }

            var workResponse = this.workQueue.AddWork(id);
            if (workResponse == WorkResponse.InProcess || workResponse == WorkResponse.Error)
            {
                return null;
            }

            return null;
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
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<TvShow> ReadAllByGenre(Genre genre)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get one television show by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public TvShow ReadByName(string name)
        {
            return ((ITvShowsRepository)this.Repository).ReadByName(name);
        }
    }
}
