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

            // Episode Routes
            routes.MapRoute(
                "Episode_Show",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}",
                new { controller = "Episodes", action = "Index" });

            routes.MapRoute(
                "Episode_Create_Comment",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}/Comments/Create",
                new { controller = "Episodes", action = "CreateComment" });

            routes.MapRoute(
                "Episode_Comment",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}/Comments/{id}",
                new { controller = "Episodes", action = "Comment" });

            routes.MapRoute(
                "Episode_Comment_Remove",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}/Comments/{id}/Remove",
                new { controller = "Episodes", action = "RemoveComment" });

            routes.MapRoute(
                "Episodes_Default",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}/{action}",
                new { controller = "Episodes" });

            routes.MapRoute("Genre_Search_Similar", "Genres/Search", new { controller = "Genres", action = "GetSimilar" });

            routes.MapRoute("Season_Index", "TvShows/{tvshowId}/Seasons/{seasonNumber}", new { controller = "Seasons", action = "Index" });

            routes.MapRoute("TvShows_Names", "TvShows/Names", new { controller = "TvShows", action = "GetNames" });
            routes.MapRoute("TvShows_Create_Comments", "TvShows/{tvshowId}/Comments/Create", new { controller = "TvShows", action = "CreateComment" });
            routes.MapRoute("TvShows_Comment", "TvShows/{tvshowId}/Comments/{id}", new { controller = "TvShows", action = "Comment" });
            routes.MapRoute("TvShows_Comment_Remove", "TvShows/{tvshowId}/Comments/{id}/Remove", new { controller = "TvShows", action = "RemoveComment" });
            routes.MapRoute("TvShows_GetByName", "TvShows", new { controller = "TvShows", action = "GetByName" });

            routes.MapRoute("Home_Main", string.Empty, new { action = "Index", controller = "Home" });
            routes.MapRoute("Home_Api", "Api", new { action = "Api",  controller = "Home" });

            routes.MapRoute("User_Main", "User", new { controller = "User", action = "Index" });
            routes.MapRoute("Users_Search", "Users/Search", new { controller = "Users", action = "Search" });

            routes.MapRoute("Default_3", "{controller}/{action}/{id}", new { }, new { controller = "User|Account" });
            routes.MapRoute("Default_2", "{controller}/{action}", new { }, new { controller = "User|Account" });
            routes.MapRoute("Default_1", "{controller}/{id}", new { action = "Index", id = UrlParameter.Optional });

            routes.MapRoute("TvShows_Users_Default", "{controller}/{id}/{action}", null, new { controller = "TvShows|Users" });
        }
    }
}