﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleForUnitTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Ninject module for STracker dependencies.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests
{
    using System.Configuration;

    using MongoDB.Driver;

    using Ninject.Modules;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.SeasonsOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Operations.EpisodesOperations;
    using STrackerServer.BusinessLayer.Operations.SeasonsOperations;
    using STrackerServer.BusinessLayer.Operations.TvShowsOperations;
    using STrackerServer.BusinessLayer.Operations.UsersOperations;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.Core.UsersRepositories;
    using STrackerServer.Repository.MongoDB.Core.EpisodesRepositories;
    using STrackerServer.Repository.MongoDB.Core.SeasonsRepositories;
    using STrackerServer.Repository.MongoDB.Core.TvShowsRepositories;
    using STrackerServer.Repository.MongoDB.Core.UsersRepositories;

    /// <summary>
    /// The module for unit tests.
    /// </summary>
    public class ModuleForUnitTests : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
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

            // Users stuff dependencies...
            this.Bind<IUsersOperations>().To<UsersOperations>();
            this.Bind<IUsersRepository>().To<UsersRepository>();
        }
    }
}