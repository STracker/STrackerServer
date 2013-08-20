// --------------------------------------------------------------------------------------------------------------------
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
            /*
              * Custom routes.
              * These routes are explicitly defined to have an overview of all available routes in Api.
              */

            // Routes for genres. -> use the default route.
            // config.Routes.MapHttpRoute("api_genres", "api/genres/{id}", new { controller = "genres", id = RouteParameter.Optional });

            // Routes for top rated tvshows.
            config.Routes.MapHttpRoute("api_tvshows_toprated", "api/tvshows/toprated", new { controller = "topratedtvshows" });

            // Routes for tvshows. -> use the default route.
            // config.Routes.MapHttpRoute("api_tvshows", "api/tvshows/{id}", new { controller = "tvshows", id = RouteParameter.Optional });

            // Routes for seasons.
            config.Routes.MapHttpRoute("api_seasons", "api/tvshows/{tvshowId}/seasons/{number}", new { controller = "seasons", number = RouteParameter.Optional });

            // Routes for episodes.
            config.Routes.MapHttpRoute("api_episodes", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}", new { controller = "episodes", number = RouteParameter.Optional });

            // Routes for tvshow new episodes.
            config.Routes.MapHttpRoute("api_tvshows_newepisodes", "api/tvshows/{tvshowId}/newepisodes", new { controller = "newepisodes", date = UrlParameter.Optional });

            // Routes for tvshows and episodes comments.
            config.Routes.MapHttpRoute("api_tvshows_comments", "api/tvshows/{id}/comments/{commentId}", new { controller = "tvshowscomments", commentId = RouteParameter.Optional });
            config.Routes.MapHttpRoute("api_episodes_comments", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}/comments/{commentId}", new { controller = "episodescomments", commentId = RouteParameter.Optional });

            // Routes for tvshows and episodes ratings.
            config.Routes.MapHttpRoute("api_tvshows_ratings", "api/tvshows/{id}/ratings", new { controller = "tvshowsratings" });
            config.Routes.MapHttpRoute("api_episodes_ratings", "api/tvshows/{tvshowId}/seasons/{seasonNumber}/episodes/{number}/ratings", new { controller = "episodesratings" });

            // Routes for users.
            // config.Routes.MapHttpRoute("api_users", "api/users/{id}", new { controller = "users", id = RouteParameter.Optional });

            // Routes for user friends.
            config.Routes.MapHttpRoute("api_user_friends", "api/user/friends/{userId}", new { controller = "userfriends", userId = RouteParameter.Optional });

            // Routes for user friend requests.
            config.Routes.MapHttpRoute("api_user_friendrequests", "api/user/friendrequests/{userId}", new { controller = "userfriendrequests", userId = RouteParameter.Optional });

            // Routes for user subscriptions.
            config.Routes.MapHttpRoute("api_user_subscriptions", "api/user/subscriptions/{tvshowId}", new { controller = "usersubscriptions", tvshowId = RouteParameter.Optional });

            // Routes for user suggestions.
            config.Routes.MapHttpRoute("api_user_suggestions", "api/user/suggestions/{tvshowId}/", new { controller = "usersuggestions", tvshowId = RouteParameter.Optional });

            // Routes for user watched episodes from the suggestions.
            config.Routes.MapHttpRoute("api_user_watchedepisodes", "api/user/subscriptions/{tvshowId}/watchedepisodes/{seasonNumber}/{episodeNumber}", new { controller = "userwatchedepisodes" });

            // Routes for user calendar.
            config.Routes.MapHttpRoute("api_user_calendar", "api/user/calendar", new { controller = "usercalendar" });

            // Routes for system
            // config.Routes.MapHttpRoute("api_system_time", "api/system/time", new { controller = "system", action = "GetTime" });

            // Default route.
            config.Routes.MapHttpRoute("api_default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
        }
    }
}