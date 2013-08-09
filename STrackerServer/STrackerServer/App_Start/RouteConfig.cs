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

            // TvShow Routes
            routes.MapRoute("TvShowsWeb_Names", "TvShows/Names", new { controller = "TvShowsWeb", action = "GetNames" });

            routes.MapRoute("TvShowsWeb_Comments", "TvShows/{tvshowId}/Comments", new { controller = "TvShowsWeb", action = "Comments" });

            routes.MapRoute("TvShowsWeb_Suggestion", "TvShows/{tvshowId}/Suggest", new { controller = "TvShowsWeb", action = "Suggest" });

            routes.MapRoute("TvShowsWeb_Subscribe", "TvShows/{tvshowId}/Subscribe", new { controller = "TvShowsWeb", action = "Subscribe" });

            routes.MapRoute("TvShowsWeb_Rate", "TvShows/{tvshowId}/Rate", new { controller = "TvShowsWeb", action = "Rate" });

            routes.MapRoute("TvShowsWeb_Create_Comments", "TvShows/{tvshowId}/Comments/Create", new { controller = "TvShowsWeb", action = "CreateComment" });

            routes.MapRoute("TvShowsWeb_Comment", "TvShows/{tvshowId}/Comments/{id}", new { controller = "TvShowsWeb", action = "Comment" });

            routes.MapRoute("TvShowsWeb_Comment_Remove", "TvShows/{tvshowId}/Comments/{id}/Remove", new { controller = "TvShowsWeb", action = "RemoveComment" }); 

            routes.MapRoute("TvShowsWeb_GetByName", "TvShows", new { controller = "TvShowsWeb", action = "GetByName" });

            routes.MapRoute(
                "TvShowsWeb_Show", 
                "TvShows/{tvshowId}", 
                new { controller = "TvShowsWeb", action = "Show" });

            routes.MapRoute(
                "SeasonWeb_Show",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}",
                new { controller = "SeasonsWeb", action = "Show" });

            // Episode Routes
            routes.MapRoute(
                "EpisodeWeb_Show",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}",
                new { controller = "EpisodesWeb", action = "Show" });

            routes.MapRoute(
                "EpisodeWeb_Comments",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}/Comments",
                new { controller = "EpisodesWeb", action = "Comments" });

            routes.MapRoute(
                "EpisodeWeb_Rate",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}/Rate",
                new { controller = "EpisodesWeb", action = "Rate" });

            routes.MapRoute(
                "EpisodeWeb_Watched",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}/Watched",
                new { controller = "EpisodesWeb", action = "Watched" });

            routes.MapRoute(
                "EpisodeWeb_Create_Comment",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}/Comments/Create",
                new { controller = "EpisodesWeb", action = "CreateComment" });

            routes.MapRoute(
                "EpisodeWeb_Comment",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}/Comments/{id}",
                new { controller = "EpisodesWeb", action = "Comment" });

            routes.MapRoute(
                "EpisodeWeb_Comment_Remove",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{episodeNumber}/Comments/{id}/Remove",
                new { controller = "EpisodesWeb", action = "RemoveComment" });

            // Defaults
            routes.MapRoute("Home_Main", string.Empty, new { action = "Index", controller = "Home" });
            routes.MapRoute("Home_Api", "Api", new { action = "Api",  controller = "Home" });

            routes.MapRoute("User_Main", "User", new { controller = "User", action = "Index" });
            routes.MapRoute("Users_Search", "Users/Search", new { controller = "Users", action = "Search" });

            routes.MapRoute("Default_2", "{controller}/{action}/{id}", new { }, new { controller = "User|Account" });
            routes.MapRoute("Default_3", "{controller}/{action}", new { }, new { controller = "User|Account" });
            routes.MapRoute("Default_1", "{controller}/{id}", new { action = "Index", id = UrlParameter.Optional });

            routes.MapRoute("Users_Default", "Users/{id}/{action}", new { controller = "Users" });
        }
    }
}