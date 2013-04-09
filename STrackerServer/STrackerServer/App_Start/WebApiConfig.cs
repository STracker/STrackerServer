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
            var appJsonFormatter = config.Formatters.JsonFormatter;
            config.Formatters.Clear();
            config.Formatters.Add(appJsonFormatter);

            config.Routes.MapHttpRoute("tvshow_get_api", "api/tvshows/{id}", new { controller = "apitvshow", action = "get" });

            config.Routes.MapHttpRoute("season_get_api", "api/tvshows/{tvshowId}/seasons/{number}", new { controller = "apiseason", action = "get" });

            config.Routes.MapHttpRoute("episode_get_api", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}", new { controller = "apiepisode", action = "get" });
        }
    }
}
