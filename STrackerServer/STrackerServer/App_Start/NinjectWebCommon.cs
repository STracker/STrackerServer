// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NinjectWebCommon.cs" company="NInject">
//   NInject.
// </copyright>
// <summary>
//   Defines the NinjectWebCommon type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

[assembly: WebActivator.PreApplicationStartMethod(typeof(STrackerServer.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(STrackerServer.App_Start.NinjectWebCommon), "Stop")]

namespace STrackerServer.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Dependencies;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using MongoDB.Driver;

    using Ninject;
    using Ninject.Syntax;
    using Ninject.Web.Common;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.BusinessLayer.Operations;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.Repository.MongoDB.Core;

    /// <summary>
    /// The NINJECT web common.
    /// </summary>
    public static class NinjectWebCommon 
    {
        /// <summary>
        /// The boots trapper.
        /// </summary>
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);

            // Install our Ninject-based IDependencyResolver into the Web API config
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // MongoDB stuff dependencies...
            kernel.Bind<MongoUrl>().ToSelf().InSingletonScope().WithConstructorArgument("url", ConfigurationManager.AppSettings["MongoDBURL"]);

            // MongoClient class is thread safe.
            kernel.Bind<MongoClient>().ToSelf().InSingletonScope().WithConstructorArgument("url", kernel.Get<MongoUrl>());

            // Television shows stuff dependencies...
            kernel.Bind<ITvShowsOperations>().To<TvShowsOperations>().InRequestScope();
            kernel.Bind<ITvShowsRepository>().To<TvShowsRepository>().InRequestScope();

            // Seasons stuff dependencies...
            kernel.Bind<ISeasonsOperations>().To<SeasonsOperations>().InRequestScope();
            kernel.Bind<ISeasonsRepository>().To<SeasonsRepository>().InRequestScope();

            // Episodes stuff dependencies...
            kernel.Bind<IEpisodesOperations>().To<EpisodesOperations>().InRequestScope();
            kernel.Bind<IEpisodesRepository>().To<EpisodesRepository>().InRequestScope();
        }

        /*
         * 
         * Adictional stuff for Web API ApiControllers.
        */

        /// <summary>
        /// Provides a NINJECT implementation of IDependencyScope
        /// which resolves services using the NINJECT container.
        /// </summary>
        internal class NinjectDependencyScope : IDependencyScope
        {
            /// <summary>
            /// The dispose message.
            /// </summary>
            private const string DisposeMessage = "This scope has been disposed";

            /// <summary>
            /// The resolver.
            /// </summary>
            private IResolutionRoot resolver;

            /// <summary>
            /// Initializes a new instance of the <see cref="NinjectDependencyScope"/> class.
            /// </summary>
            /// <param name="resolver">
            /// The resolver.
            /// </param>
            public NinjectDependencyScope(IResolutionRoot resolver)
            {
                this.resolver = resolver;
            }

            /// <summary>
            /// The get service.
            /// </summary>
            /// <param name="serviceType">
            /// The service type.
            /// </param>
            /// <returns>
            /// The <see cref="object"/>.
            /// </returns>
            public object GetService(Type serviceType)
            {
                if (this.resolver == null)
                {
                    throw new ObjectDisposedException("this", DisposeMessage);
                }

                return this.resolver.TryGet(serviceType);
            }

            /// <summary>
            /// The get services.
            /// </summary>
            /// <param name="serviceType">
            /// The service type.
            /// </param>
            /// <returns>
            /// The <see cref="IEnumerable{T}"/>.
            /// </returns>
            public IEnumerable<object> GetServices(Type serviceType)
            {
                if (this.resolver == null)
                {
                    throw new ObjectDisposedException("this", DisposeMessage);
                }

                return this.resolver.GetAll(serviceType);
            }

            /// <summary>
            /// The dispose.
            /// </summary>
            public void Dispose()
            {
                var disposable = this.resolver as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }

                this.resolver = null;
            }
        }

        /// <summary>
        /// This class is the resolver, but it is also the global scope
        /// so we derive from NINJECT Scope.
        /// </summary>
        internal class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
        {
            /// <summary>
            /// The kernel.
            /// </summary>
            private readonly IKernel kernel;

            /// <summary>
            /// Initializes a new instance of the <see cref="NinjectDependencyResolver"/> class.
            /// </summary>
            /// <param name="kernel">
            /// The kernel.
            /// </param>
            public NinjectDependencyResolver(IKernel kernel)
                : base(kernel)
            {
                this.kernel = kernel;
            }

            /// <summary>
            /// The begin scope.
            /// </summary>
            /// <returns>
            /// The <see cref="IDependencyScope"/>.
            /// </returns>
            public IDependencyScope BeginScope()
            {
                return new NinjectDependencyScope(kernel.BeginBlock());
            }
        }
    }
}