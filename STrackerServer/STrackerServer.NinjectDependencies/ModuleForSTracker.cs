// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleForSTracker.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Ninject module for STracker dependencies.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.NinjectDependencies
{
    using System.Configuration;

    using MongoDB.Driver;

    using Ninject.Modules;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.BusinessLayer.Operations;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.InformationProviders.Core;
    using STrackerServer.InformationProviders.Providers;
    using STrackerServer.Repository.MongoDB.Core;
    using STrackerServer.WorkQueue;
    using STrackerServer.WorkQueue.Core;

    /// <summary>
    /// The module for STRACKER.
    /// </summary>
    public class ModuleForSTracker : NinjectModule 
    {
        /// <summary>
        /// The load.
        /// </summary>
        public override void Load()
        {
            // MongoDB stuff dependencies...
            this.Bind<MongoUrl>().ToSelf().InSingletonScope().WithConstructorArgument("url", ConfigurationManager.AppSettings["MongoDBURL"]);

            // MongoClient class is thread safe.
            this.Bind<MongoClient>().ToSelf().InSingletonScope();

            // Television shows stuff dependencies...
            this.Bind<ITvShowsOperations>().To<TvShowsOperations>();
            this.Bind<ITvShowsRepository>().To<TvShowsRepository>();

            // Seasons stuff dependencies...
            this.Bind<ISeasonsOperations>().To<SeasonsOperations>();
            this.Bind<ISeasonsRepository>().To<SeasonsRepository>();

            // Episodes stuff dependencies...
            this.Bind<IEpisodesOperations>().To<EpisodesOperations>();
            this.Bind<IEpisodesRepository>().To<EpisodesRepository>();

            // Users stuff dependencies...
            this.Bind<IUsersOperations>().To<UsersOperations>();
            this.Bind<IUsersRepository>().To<UsersRepository>();

            // Work queues stuff dependencies...
            this.Bind<IQueue<string, CreateTvShowsWork>>().To<QueueForCreateTvShowsWorks>().InSingletonScope();
            this.Bind<IWorkQueueForTvShows>().To<WorkQueueForTvShows>();

            // External providers stuff dependencies...
            this.Bind<ITvShowsInformationProvider>().To<TheTvDbProvider>();
        }
    }
}
