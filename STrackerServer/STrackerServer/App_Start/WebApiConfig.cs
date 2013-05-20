// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.App_Start
{
    using System.Web.Http;

    /// <summary>
    /// Web API config.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register method.
        /// </summary>
        /// <param name="config">
        /// HTTP config.
        /// </param>
        public static void Register(HttpConfiguration config)
        {
            // Routes for tvshows.
            config.Routes.MapHttpRoute("tvshow_get", "api/tvshows/{id}", new { controller = "tvshows", action = "get" });
            config.Routes.MapHttpRoute("tvshow_geAllByGenre", "api/tvshows", new { controller = "tvshows" });

            // Routes for seasons.
            config.Routes.MapHttpRoute("season_getAll", "api/tvshows/{tvshowId}/seasons", new { controller = "seasons", action = "getall" });
            config.Routes.MapHttpRoute("season_get", "api/tvshows/{tvshowId}/seasons/{number}", new { controller = "seasons", action = "get" });

            // Routes for episodes.
            config.Routes.MapHttpRoute("episode_getall", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes", new { controller = "episodes", action = "getall" });
            config.Routes.MapHttpRoute("episode_get", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}", new { controller = "episodes", action = "get" });
        }
    }
}
