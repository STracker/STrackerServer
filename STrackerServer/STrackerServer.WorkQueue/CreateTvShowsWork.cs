// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTvShowsWork.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  CreateTvShowsWork object that encapsulates the work. Contains an event that is signaled 
//  in the conclusion of the work.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.WorkQueue
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.InformationProviders.Core;
    using STrackerServer.WorkQueue.Core;

    /// <summary>
    /// The work.
    /// </summary>
    public class CreateTvShowsWork : IWork<TvShow, string>
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
        /// The wait event.
        /// </summary>
        private readonly ManualResetEvent waitEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTvShowsWork"/> class.
        /// </summary>
        /// <param name="tvshowsRepository">
        /// The television shows Repository.
        /// </param>
        /// <param name="seasonsRepository">
        /// The seasons Repository.
        /// </param>
        /// <param name="episodesRepository">
        /// The episodes Repository.
        /// </param>
        /// <param name="infoProvider">
        /// The info Provider.
        /// </param>
        /// <param name="imdbId">
        /// The IMDB Id.
        /// </param>
        public CreateTvShowsWork(ITvShowsRepository tvshowsRepository, ISeasonsRepository seasonsRepository, IEpisodesRepository episodesRepository, ITvShowsInformationProvider infoProvider, string imdbId)
        {
            this.tvshowsRepository = tvshowsRepository;
            this.seasonsRepository = seasonsRepository;
            this.episodesRepository = episodesRepository;
            this.infoProvider = infoProvider;
            this.waitEvent = new ManualResetEvent(false);
            this.Id = imdbId;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The begin execute work.
        /// </summary>
        public void BeginExecute()
        {
            var task = Task.Factory.StartNew(this.ExecuteTask);

            task.ContinueWith(
                complete =>
                {
                    complete.Wait();
                    this.waitEvent.Set();
                });
        }

        /// <summary>
        /// The end execute work.
        /// </summary>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public TvShow EndExecute()
        {
            this.waitEvent.WaitOne();

            return this.tvshowsRepository.Read(this.Id);
        }

        /// <summary>
        /// The execute task.
        /// </summary>
        private void ExecuteTask()
        {
            // Verify if the television show realy exists.
            if (!this.infoProvider.VerifyIfExists(this.Id))
            {
                return;
            }

            var tvshowInfo = this.infoProvider.GetTvShowInformation(this.Id);
            if (tvshowInfo == null || !this.tvshowsRepository.Create(tvshowInfo))
            {
                return;
            }
            
            var seasonsInfo = this.infoProvider.GetSeasonsInformation(this.Id);
            var enumerable = seasonsInfo as List<Season> ?? seasonsInfo.ToList();
            if (seasonsInfo == null || !this.seasonsRepository.CreateAll(enumerable))
            {
                return;
            }

            foreach (var episodesInfo in enumerable.Select(season => this.infoProvider.GetEpisodesInformation(this.Id, season.SeasonNumber)).Where(episodesInfo => episodesInfo != null))
            {
                this.episodesRepository.CreateAll(episodesInfo);
            }
        }
    }
}
