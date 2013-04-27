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
    public class CreateTvShowsWork : IWork<TvShow>
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
        /// The event.
        /// </summary>
        private readonly AutoResetEvent myEvent;

        /// <summary>
        /// The IMDB id.
        /// </summary>
        private readonly string imdbId;

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
            this.imdbId = imdbId;
            this.myEvent = new AutoResetEvent(false);
        }

        /// <summary>
        /// The begin execute work.
        /// </summary>
        public void BeginExecuteWork()
        {
            // Verify if the television show realy exists.
            if (!this.infoProvider.VerifyIfExists(this.imdbId))
            {
                return;
            }

            var task = Task.Factory.StartNew(this.ExecuteTask);

            task.ContinueWith(
                complete =>
                {
                    complete.Wait();
                    this.myEvent.Set();
                });
        }

        /// <summary>
        /// The end execute work.
        /// </summary>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public TvShow EndExecuteWork()
        {
            this.myEvent.WaitOne();
            return this.tvshowsRepository.Read(this.imdbId);
        }

        /// <summary>
        /// The execute task.
        /// </summary>
        private void ExecuteTask()
        {
            var tvshowInfo = this.infoProvider.GetTvShowInformation(this.imdbId);
            if (tvshowInfo == null || !this.tvshowsRepository.Create(tvshowInfo))
            {
                return;
            }
            
            var seasonsInfo = this.infoProvider.GetSeasonsInformation(tvshowInfo);
            var enumerable = seasonsInfo as List<Season> ?? seasonsInfo.ToList();
            if (seasonsInfo == null || !this.seasonsRepository.CreateAll(enumerable))
            {
                return;
            }

            var episodesInfo = this.infoProvider.GetEpisodesInformation(tvshowInfo);
            if (episodesInfo != null)
            {
                this.episodesRepository.CreateAll(episodesInfo);    
            }

            var xpto = 0;
        }
    }
}
