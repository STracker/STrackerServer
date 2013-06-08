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

            // Root Routes
            routes.MapRoute("Home", string.Empty, new { controller = "HomeWeb", action = "Index" });
            routes.MapRoute("Contact", "Contact", new { controller = "HomeWeb", action = "Contact" });

            // User Routes
            routes.MapRoute("User_Requests_Response", "User/Requests/{id}", new { controller = "UsersWeb", action = "RequestResponse" });
            routes.MapRoute("User_Requests", "User/Requests", new { controller = "UsersWeb", action = "Requests" });
            routes.MapRoute("User_Invite", "User/Invite", new { controller = "UsersWeb", action = "Invite" });
            routes.MapRoute("User_Subscribe", "User/Subscribe", new { controller = "UsersWeb", action = "Subscribe" });
            routes.MapRoute("User_UnSubscribe", "User/UnSubscribe", new { controller = "UsersWeb", action = "UnSubscribe" });
            routes.MapRoute("User_Show", "User/{id}", new { controller = "UsersWeb", action = "Show" });

            routes.MapRoute("User_Index", "User", new { controller = "UsersWeb", action = "Index" });

            // Account Routes
            routes.MapRoute(
                "Account_Login", "Account/Login", new { controller = "Account", action = "Login" });

            routes.MapRoute(
                "Account_Callback",
                "Account/Callback",
                new
                    {
                        controller = "Account",
                        action = "Callback",
                        code = UrlParameter.Optional,
                        error = UrlParameter.Optional,
                        error_code = UrlParameter.Optional,
                        error_description = UrlParameter.Optional,
                        error_reason = UrlParameter.Optional
                    });

            routes.MapRoute("Account_Logout", "Account/Logout", new { controller = "Account", action = "Logout" });

            // TvShow Routes
            routes.MapRoute("TvShowsWeb_Comments", "TvShows/{tvshowId}/Comments", new { controller = "TvShowsWeb", action = "Comments" });

            routes.MapRoute("TvShowsWeb_Create_Comments", "TvShows/{tvshowId}/Comments/Create", new { controller = "TvShowsWeb", action = "CreateComment" });
            
            routes.MapRoute("TvShowsWeb_Comment", "TvShows/{tvshowId}/Comments/{position}", new { controller = "TvShowsWeb", action = "CommentsEdit" });

            routes.MapRoute("TvShowsWeb_Comment_Remove", "TvShows/{tvshowId}/Comments/{position}/Remove", new { controller = "TvShowsWeb", action = "RemoveComment" }); 

            routes.MapRoute("TvShowsWeb_GetByName", "TvShows", new { controller = "TvShowsWeb", action = "GetByName" });

            routes.MapRoute(
                "TvShowsWeb_Show", 
                "TvShows/{tvshowId}", 
                new { controller = "TvShowsWeb", action = "Show" });

            routes.MapRoute(
                "SeasonWeb_Show",
                "TvShows/{tvshowId}/Seasons/{number}",
                new { controller = "SeasonsWeb", action = "Show" });

            routes.MapRoute(
                "EpisodeWeb_Show",
                "TvShows/{tvshowId}/Seasons/{seasonNumber}/Episodes/{number}",
                new { controller = "EpisodesWeb", action = "Show" });
        }
    }
}