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

            routes.MapRoute("home", string.Empty, new { controller = "Home", action = "Index" });

            routes.MapRoute("tvshow_get", "tvshow/{tvshowId}", new { controller = "TvShow", action = "Get", format = "html" });
            routes.MapRoute("tvshow_get_api", "tvshow/{tvshowId}/api", new { controller = "TvShow", action = "Get", format = "json" });

            routes.MapRoute("season_get", "tvshow/{tvshowId}/season/{seasonNumber}", new { controller = "Season", action = "Get", format = "html" });
            routes.MapRoute("season_get_api", "tvshow/{tvshowId}/season/{seasonNumber}/api", new { controller = "Season", action = "Get", format = "json" });
        }
    }
}