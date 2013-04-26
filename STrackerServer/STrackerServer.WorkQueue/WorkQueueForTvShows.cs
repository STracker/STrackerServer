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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
        /// The work items repository.
        /// </summary>
        private readonly ITvShowsWorkItemsRepository workItemsRepository;

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
        /// Initializes a new instance of the <see cref="WorkQueueForTvShows"/> class.
        /// </summary>
        /// <param name="workItemsRepository">
        /// The work items repository.
        /// </param>
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
        public WorkQueueForTvShows(ITvShowsWorkItemsRepository workItemsRepository, ITvShowsRepository tvshowsRepository, ISeasonsRepository seasonsRepository, IEpisodesRepository episodesRepository, ITvShowsInformationProvider infoProvider)
        {
            this.workItemsRepository = workItemsRepository;
            this.tvshowsRepository = tvshowsRepository;
            this.seasonsRepository = seasonsRepository;
            this.episodesRepository = episodesRepository;
            this.infoProvider = infoProvider;
        }

        /// <summary>
        /// Add one work to queue.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The <see cref="WorkResponse"/>.
        /// </returns>
        public WorkResponse AddWork(params object[] parameters)
        {
            var imdbId = (string)parameters[0];

            // Verifiy if the television show exists before creating the task.
            if (!this.infoProvider.VerifyIfExists(imdbId))
            {
                return WorkResponse.Error;
            }

            var workItem = new TvShowWorkItem { Key = imdbId };

            // Test for just in case of two or more requests make at same time
            // the request for creating a work item for same television show.
            if (!this.workItemsRepository.Create(workItem))
            {
                return WorkResponse.InProcess;
            }

            // Creating the task.
            var task = Task.Factory.StartNew(() => this.TaskWork(imdbId));
            task.ContinueWith(
                completed =>
                    {
                        completed.Wait();

                        // Delete document of the work from database when the work is completed.
                        this.workItemsRepository.Delete(imdbId);
                    });

            return WorkResponse.InProcess;
        }

        /// <summary>
        /// Verify if already exists the work.
        /// </summary>
        /// <param name="workId">
        /// The work id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ExistsWork(object workId)
        {
            return this.workItemsRepository.Read(string.Format("{0}", workId)) != null;
        }

        /// <summary>
        /// The task work.
        /// </summary>
        /// <param name="imdbId">
        /// The id.
        /// </param>
        private void TaskWork(string imdbId)
        {
            /*
             * Get information from external providers.
             */

            // First try get the television show basic information.
            var tvshow = this.infoProvider.GetTvShowInformation(imdbId);
            if (!this.tvshowsRepository.Create(tvshow))
            {
                return;
            }

            // Then try get the seasons.
            var seasons = this.infoProvider.GetSeasonsInformation(imdbId);
            var enumerable = seasons as List<Season> ?? seasons.ToList();
            if (!this.seasonsRepository.CreateAll(enumerable))
            {
                return;
            }

            // Then try get the episodes.
            foreach (var episodes in enumerable.Select(season => this.infoProvider.GetEpisodesInformation(imdbId, season.SeasonNumber)))
            {
                this.episodesRepository.CreateAll(episodes);
            }
        }
    }
}