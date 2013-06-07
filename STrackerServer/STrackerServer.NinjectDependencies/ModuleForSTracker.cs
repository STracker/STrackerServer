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

    using RabbitMQ.Client;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.BusinessLayer.Operations;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.Repository.MongoDB.Core;

    using STrackerUpdater.RabbitMQ;

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

            // Comments stuff dependencies...
            this.Bind<ITvShowCommentsRepository>().To<TvShowCommentsRepository>();
            this.Bind<IEpisodeCommentsRepository>().To<EpisodeCommentsRepository>();
            this.Bind<ITvShowsCommentsOperations>().To<TvShowsCommentsOperations>();
            this.Bind<IEpisodesCommentsOperations>().To<EpisodesCommentsOperations>();

            // Ratings stuff dependencies...
            this.Bind<ITvShowRatingsRepository>().To<TvShowRatingsRepository>();
            this.Bind<IEpisodeRatingsRepository>().To<EpisodeRatingsRepository>();
            this.Bind<ITvShowsRatingsOperations>().To<TvShowsRatingsOperations>();
            this.Bind<IEpisodesRatingsOperations>().To<EpisodesRatingsOperations>();

            // Friends stuff dependencies...
            this.Bind<IFriendRequestOperations>().To<FriendRequestOperations>();
            this.Bind<IFriendRequestRepository>().To<FriendRequestRepository>();

            // Queue dependencies...
            this.Bind<ConnectionFactory>().ToSelf().InSingletonScope().WithPropertyValue("Uri", ConfigurationManager.AppSettings["RabbitMQUri"]);
            this.Bind<QueueManager>().ToSelf().InSingletonScope();
        }
    }
}
