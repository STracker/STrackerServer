// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.TvShowsOperations
{
    using System.Collections.Generic;
    using System.Configuration;

    using STrackerBackgroundWorker.RabbitMQ;
    using STrackerBackgroundWorker.RabbitMQ.Core;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// Television shows operations.
    /// </summary>
    public class TvShowsOperations : BaseCrudOperations<ITvShowsRepository, TvShow, string>, ITvShowsOperations
    {
        /// <summary>
        /// The queue manager.
        /// </summary>
        private readonly QueueManager queueM;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="queueM">
        /// The queue manager.
        /// </param>
        public TvShowsOperations(ITvShowsRepository repository, QueueManager queueM)
            : base(repository)
        {
            this.queueM = queueM;
        }

        /// <summary>
        /// Get one television show by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="STrackerServer.DataAccessLayer.DomainEntities.TvShow"/>.
        /// </returns>
        public override TvShow Read(string id)
        {
            var tvshow = this.Repository.Read(id);
            if (tvshow != null)
            {
                return tvshow;
            }

            this.queueM.Push(new Message { CommandName = ConfigurationManager.AppSettings["TvShowAddByIdCmd"], Arg = id });
            return null;
        }

        /// <summary>
        /// The read by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="range">
        /// The range.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<TvShow.TvShowSynopsis> ReadByName(string name, Range range = null)
        {
            if (name == null || string.Empty.Equals(name))
            {
                return new List<TvShow.TvShowSynopsis>();
            }

            var tvshows = this.Repository.ReadByName(name);

            if (tvshows.Count == 0)
            {
                this.queueM.Push(new Message { CommandName = ConfigurationManager.AppSettings["TvShowAddByNameCmd"], Arg = name });
            }

            return tvshows.ApplyRange(range);
        }

        /// <summary>
        /// Get a list of television shows with name equals to name in parameters directly to the database
        /// to avoid calls to the background worker.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<TvShow.TvShowSynopsis> DirectReadByName(string name)
        {
            if (name == null || string.Empty.Equals(name))
            {
                return new List<TvShow.TvShowSynopsis>();
            }

            return this.Repository.ReadByName(name);
        }
    }
}