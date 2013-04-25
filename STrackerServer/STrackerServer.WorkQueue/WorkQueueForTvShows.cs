// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkQueueForTvShows.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IWorkQueueForTvShows interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.WorkQueue
{
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
        private readonly IInformationProvider infoProvider;

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
        public WorkQueueForTvShows(ITvShowsWorkItemsRepository workItemsRepository, ITvShowsRepository tvshowsRepository, ISeasonsRepository seasonsRepository, IEpisodesRepository episodesRepository, IInformationProvider infoProvider)
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
            var tvshowId = (string)parameters[0];
            var workItem = new TvShowWorkItem { Key = tvshowId };

            if (!this.workItemsRepository.Create(workItem))
            {
                return WorkResponse.InProcess;
            }

            // Verifiy if the television show exists before creating the task.
            if (!this.infoProvider.VerifyIfExists(tvshowId))
            {
                return WorkResponse.Error;
            }

            var task = Task.Factory.StartNew(() => this.TaskWork(tvshowId));
            task.ContinueWith(
                completed =>
                    {
                        completed.Wait();
                        if (!workItemsRepository.Delete(tvshowId))
                        {
                            // TODO, error while deleting, necessary to add to log.
                        }
                    });

            return WorkResponse.InProcess;
        }

        /// <summary>
        /// The task work.
        /// </summary>
        /// <param name="tvshowId">
        /// The id.
        /// </param>
        private void TaskWork(string tvshowId)
        {
            /*
             * Get information from external providers.
             */

            // First try get the television show basic information.
            var tvshow = this.infoProvider.GetTvShowInformationByImdbId(tvshowId);
            if (!this.tvshowsRepository.Create(tvshow))
            {
                // TODO, add error to log.
                return;
            }

            // Then try get seasons from the desire television show.
            var seasons = this.infoProvider.GetSeasons(tvshowId);

            if (this.seasonsRepository.CreateAll(seasons))
            {
                // TODO, add error to log.
                return;
            }

            // Finaly try get the episodes.
            var episodes = this.infoProvider.GetEpisodes()
        }
    }
}
