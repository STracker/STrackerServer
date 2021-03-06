﻿// --------------------------------------------------------------------------------------------------------------------
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

    using CloudinaryDotNet;

    using MongoDB.Driver;

    using Ninject.Modules;

    using RabbitMQ.Client;

    using STrackerBackgroundWorker.RabbitMQ;

    using STrackerServer.BusinessLayer.Calendar;
    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.SeasonsOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Operations;
    using STrackerServer.BusinessLayer.Operations.EpisodesOperations;
    using STrackerServer.BusinessLayer.Operations.SeasonsOperations;
    using STrackerServer.BusinessLayer.Operations.TvShowsOperations;
    using STrackerServer.BusinessLayer.Operations.UsersOperations;
    using STrackerServer.BusinessLayer.Permissions;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.Core.UsersRepositories;
    using STrackerServer.ImageConverter.Cloudinary;
    using STrackerServer.ImageConverter.Core;
    using STrackerServer.Logger.Core;
    using STrackerServer.Logger.SendGrid;
    using STrackerServer.Repository.MongoDB.Core;
    using STrackerServer.Repository.MongoDB.Core.EpisodesRepositories;
    using STrackerServer.Repository.MongoDB.Core.SeasonsRepositories;
    using STrackerServer.Repository.MongoDB.Core.TvShowsRepositories;
    using STrackerServer.Repository.MongoDB.Core.UsersRepositories;

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
            this.Bind<IGenresRepository>().To<GenresRepository>();
            this.Bind<IGenresOperations>().To<GenresOperations>();
            this.Bind<ITvShowsCommentsOperations>().To<TvShowsCommentsOperations>();
            this.Bind<ITvShowCommentsRepository>().To<TvShowCommentsRepository>();
            this.Bind<ITvShowsRatingsOperations>().To<TvShowsRatingsOperations>();
            this.Bind<ITvShowRatingsRepository>().To<TvShowRatingsRepository>();

            // Seasons stuff dependencies...
            this.Bind<ISeasonsOperations>().To<SeasonsOperations>();
            this.Bind<ISeasonsRepository>().To<SeasonsRepository>();

            // Episodes stuff dependencies...
            this.Bind<IEpisodesOperations>().To<EpisodesOperations>();
            this.Bind<IEpisodesRepository>().To<EpisodesRepository>();
            this.Bind<IEpisodesRatingsOperations>().To<EpisodesRatingsOperations>();
            this.Bind<IEpisodesCommentsOperations>().To<EpisodesCommentsOperations>();
            this.Bind<IEpisodeCommentsRepository>().To<EpisodeCommentsRepository>();
            this.Bind<IEpisodeRatingsRepository>().To<EpisodeRatingsRepository>();

            // New Episodes dependencies
            this.Bind<ITvShowNewEpisodesRepository>().To<TvShowNewEpisodesRepository>();
            this.Bind<ITvShowNewEpisodesOperations>().To<TvShowNewEpisodesOperations>();

            // Users stuff dependencies...
            this.Bind<IUsersOperations>().To<UsersOperations>();
            this.Bind<IUsersRepository>().To<UsersRepository>();

            // Queue dependencies...
            this.Bind<ConnectionFactory>().ToSelf().InSingletonScope().WithPropertyValue("Uri", ConfigurationManager.AppSettings["RabbitMQUri"]);
            this.Bind<QueueManager>().ToSelf().InSingletonScope();

            // PermissionProvider dependencies
            this.Bind<IPermissionManager<Permissions, int>>().To<PermissionManager>();

            // Logger dependencies
            this.Bind<ILogger>().To<SendGridLogger>();

            // Calendar dependencies
            this.Bind<ICalendar>().To<Calendar>();

            // IImagRepository dependencies
            this.Bind<IImageConverter>().To<CloudinaryConverter>();

            // Cloudinary dependencies
            this.Bind<Account>().ToSelf()
                .WithConstructorArgument("cloud", ConfigurationManager.AppSettings["Cloudinary:Cloud"])
                .WithConstructorArgument("apiKey", ConfigurationManager.AppSettings["Cloudinary:ApiKey"])
                .WithConstructorArgument("apiSecret", ConfigurationManager.AppSettings["Cloudinary:ApiSecret"]);

            this.Bind<Cloudinary>().ToSelf();
        }
    }
}
