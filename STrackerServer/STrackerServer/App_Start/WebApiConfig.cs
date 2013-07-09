﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.App_Start
{
    using System.Web.Http;
    using System.Web.Mvc;

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
            // Routes for genres.
            config.Routes.MapHttpRoute("api_genres", "api/genres", new { controller = "genres", action = "getall" });
            config.Routes.MapHttpRoute("api_genres_get", "api/genres/{id}", new { controller = "genres", action = "get" });

            // Routes for tvshows.
            config.Routes.MapHttpRoute("api_tvshows", "api/tvshows", new { controller = "tvshows" });
            config.Routes.MapHttpRoute("api_tvshows_toprated", "api/tvshows/toprated", new { controller = "tvshows", action = "GetTopRated" });
            config.Routes.MapHttpRoute("api_tvshow_get", "api/tvshows/{id}", new { controller = "tvshows", action = "get" });
            
            // Routes for seasons.
            config.Routes.MapHttpRoute("api_seasons", "api/tvshows/{tvshowId}/seasons", new { controller = "seasons", action = "getall" });
            config.Routes.MapHttpRoute("api_season_get", "api/tvshows/{tvshowId}/seasons/{number}", new { controller = "seasons", action = "get" });

            // Routes for episodes.
            config.Routes.MapHttpRoute("api_episode_newest", "api/tvshows/{tvshowId}/newestepisodes", new { controller = "episodes", action = "getnewest", date = UrlParameter.Optional });
            config.Routes.MapHttpRoute("api_episodes", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes", new { controller = "episodes", action = "getall" });
            config.Routes.MapHttpRoute("api_episode_get", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}", new { controller = "episodes", action = "get" });

            // Routes for tvshows and episodes comments.
            config.Routes.MapHttpRoute("api_tvshow_comments_delete", "api/tvshows/{id}/comments/{commentId}", new { controller = "tvshowscomments", action = "delete" });
            config.Routes.MapHttpRoute("api_tvshow_comments", "api/tvshows/{id}/comments", new { controller = "tvshowscomments" });

            config.Routes.MapHttpRoute("api_episode_comments_delete", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}/comments/{commentId}", new { controller = "episodescomments", action = "delete" });
            config.Routes.MapHttpRoute("api_episode_comments", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}/comments", new { controller = "episodescomments" });

            // Routes for tvshows and episodes ratings.
            config.Routes.MapHttpRoute("api_tvshow_ratings", "api/tvshows/{id}/ratings", new { controller = "tvshowsratings" });
            config.Routes.MapHttpRoute("api_episode_ratings", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}/ratings", new { controller = "episodesratings" });

            // Routes for users.
            config.Routes.MapHttpRoute("api_users_post", "api/users", new { controller = "users", action = "post" });
            config.Routes.MapHttpRoute("api_users_get", "api/users/{userId}", new { controller = "users", action = "get" });

            // Routes for users subscriptions.
            config.Routes.MapHttpRoute("api_subscriptions_get", "api/user/subscriptions/", new { controller = "tvshowsubscriptions", action = "get" });
            config.Routes.MapHttpRoute("api_subscriptions", "api/tvshows/{tvshowId}/subscriptions/", new { controller = "tvshowsubscriptions" });

            // Routes for system
            config.Routes.MapHttpRoute("api_system_time", "api/system/time", new { controller = "system", action = "GetTime" });

            // Default route.
            // config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
        }
    }
}