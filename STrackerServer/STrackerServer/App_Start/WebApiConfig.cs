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
            config.Routes.MapHttpRoute("api_tvshows", "api/tvshows", new { controller = "tvshows" });
            config.Routes.MapHttpRoute("api_tvshow_get", "api/tvshows/{id}", new { controller = "tvshows", action = "get" });
            config.Routes.MapHttpRoute("api_tvshow_comments", "api/tvshows/{id}/comments", new { controller = "tvshowscomments", action = "get" });
            config.Routes.MapHttpRoute("api_tvshow_ratings", "api/tvshows/{id}/ratings", new { controller = "tvshowsratings", action = "get" });

            // Routes for seasons.
            config.Routes.MapHttpRoute("api_seasons", "api/tvshows/{tvshowId}/seasons", new { controller = "seasons", action = "getall" });
            config.Routes.MapHttpRoute("api_season_get", "api/tvshows/{tvshowId}/seasons/{number}", new { controller = "seasons", action = "get" });

            // Routes for episodes.
            config.Routes.MapHttpRoute("api_episodes", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes", new { controller = "episodes", action = "getall" });
            config.Routes.MapHttpRoute("api_episode_get", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}", new { controller = "episodes", action = "get" });
            config.Routes.MapHttpRoute("api_episode_comments", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}/comments", new { controller = "episodescomments", action = "get" });
            config.Routes.MapHttpRoute("api_episode_ratings", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}/ratings", new { controller = "episodesratings", action = "get" });

            // Default route.
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
        }
    }
}