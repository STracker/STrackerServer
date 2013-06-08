﻿// --------------------------------------------------------------------------------------------------------------------
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

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    using STrackerUpdater.RabbitMQ;
    using STrackerUpdater.RabbitMQ.Core;

    /// <summary>
    /// Television shows operations.
    /// </summary>
    public class TvShowsOperations : BaseCrudOperations<TvShow, string>, ITvShowsOperations
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
        public IEnumerable<TvShow.TvShowSynopsis> ReadAllByGenre(string genre)
        {
            return ((ITvShowsRepository)this.Repository).ReadAllByGenre(genre.ToLower());
        }

        /// <summary>
        /// Try Get one television show by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="STrackerServer.DataAccessLayer.DomainEntities.TvShow"/>.
        /// </returns>
        public TvShow ReadByName(string name)
        {
            var tvshow = ((ITvShowsRepository)this.Repository).ReadByName(name);
            if (tvshow != null)
            {
                return tvshow;
            }

            this.queueM.Push(new Message { CommandName = ConfigurationManager.AppSettings["TvShowAddByNameCmd"], Arg = name });
            return null;
        }
    }
}