// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkQueueForTvShows.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IWorkQueueForTvShows interface. Using the TPL  
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.WorkQueue
{
    using System.Collections.Concurrent;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.InformationProviders.Core;
    using STrackerServer.WorkQueue.Core;

    /// <summary>
    /// The work queue for television shows.
    /// </summary>
    public class WorkQueueForTvShows : IWorkQueueForTvShows
    {
        /// <summary>
        /// The television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// The seasons repository.
        /// </summary>
        private readonly ISeasonsRepository seasonsRepository;

        /// <summary>
        /// The episodes repository.
        /// </summary>
        private readonly IEpisodesRepository episodesRepository;

        /// <summary>
        /// The information provider.
        /// </summary>
        private readonly ITvShowsInformationProvider infoProvider;

        /// <summary>
        /// The work queue.
        /// </summary>
        private readonly ConcurrentDictionary<string, CreateTvShowsWork> workQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkQueueForTvShows"/> class.
        /// </summary>
        /// <param name="tvshowsRepository">
        /// The television shows repository.
        /// </param>
        /// <param name="seasonsRepository">
        /// The seasons repository.
        /// </param>
        /// <param name="episodesRepository">
        /// The episodes repository.
        /// </param>
        /// <param name="infoProvider">
        /// The information Provider.
        /// </param>
        public WorkQueueForTvShows(ITvShowsRepository tvshowsRepository, ISeasonsRepository seasonsRepository, IEpisodesRepository episodesRepository, ITvShowsInformationProvider infoProvider)
        {
            this.tvshowsRepository = tvshowsRepository;
            this.seasonsRepository = seasonsRepository;
            this.episodesRepository = episodesRepository;
            this.infoProvider = infoProvider;
            this.workQueue = new ConcurrentDictionary<string, CreateTvShowsWork>();
        }

        /// <summary>
        /// Add one work to queue.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IWork</cref>
        ///     </see> .
        /// </returns>
        public IWork<TvShow> AddWork(params object[] parameters)
        {
            var imdbId = (string)parameters[0];
            var work = new CreateTvShowsWork(this.tvshowsRepository, this.seasonsRepository, this.episodesRepository, this.infoProvider, imdbId);
            return this.workQueue.TryAdd(imdbId, work) ? work : null;
        }

        /// <summary>
        /// Verify if already exists one same work.
        /// </summary>
        /// <param name="workId">
        /// The work id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IWork</cref>
        ///     </see> .
        /// </returns>
        public IWork<TvShow> ExistsWork(object workId)
        {
            return this.workQueue.ContainsKey((string)workId) ? this.workQueue[(string)workId] : null;
        }
    }
}