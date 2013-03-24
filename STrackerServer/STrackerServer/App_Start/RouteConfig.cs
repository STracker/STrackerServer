﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.App_Start
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// The route config.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// The register routes.
        /// </summary>
        /// <param name="routes">
        /// The routes.
        /// </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            */
            routes.MapRoute("home", "", new { controller = "Home", action = "Index" });
            routes.MapRoute("tvshow get json", "tvshow/{id}/json", new { controller = "TvShow", action = "ShowJson" });
            routes.MapRoute("tvshow get", "tvshow/{id}/html", new { controller = "TvShow", action = "Show" });
        }
    }
}