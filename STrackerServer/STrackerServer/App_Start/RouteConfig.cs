// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.App_Start
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Route config.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Register routes.
        /// </summary>
        /// <param name="routes">
        /// The routes.
        /// </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("home", string.Empty, new { controller = "Home", action = "Get" });

            routes.MapRoute("testing", "developers", new { controller = "Developers", action = "Get" });

            routes.MapRoute("users", "user", new { controller = "Users", action = "GetInfo" });

            routes.MapRoute("AccountActions", "Account/{action}", new { controller = "Account" });
        }
    }
}