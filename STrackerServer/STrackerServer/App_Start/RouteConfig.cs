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

            routes.MapRoute("Home", string.Empty, new { controller = "HomeWeb", action = "Index" });
            routes.MapRoute("Contact", "Contact", new { controller = "HomeWeb", action = "Contact" });

            routes.MapRoute("User_Index", "User", new { controller = "UsersWeb", action = "Index" });

            routes.MapRoute("User_AcceptRequest", "User/Requests/{id}", new { controller = "UsersWeb", action = "AcceptRequest" });
            routes.MapRoute("User_Requests", "User/Requests", new { controller = "UsersWeb", action = "Requests" });
      
            routes.MapRoute("User_Show", "User/{id}", new { controller = "UsersWeb", action = "Show" });

            routes.MapRoute("User_Invite", "User/{id}/Invite", new { controller = "UsersWeb", action = "Invite" });

            routes.MapRoute("User_Subscribe", "User/Subscribe/{tvshowId}", new { controller = "UsersWeb", action = "Subscribe" });
            routes.MapRoute("User_UnSubscribe", "User/UnSubscribe/{tvshowId}", new { controller = "UsersWeb", action = "UnSubscribe" });

            routes.MapRoute(
                "Account_Login",
                "Account/Login",
                new { controller = "Account", action = "Login", returnUrl = UrlParameter.Optional });

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

            routes.MapRoute("TvShowsWeb_Show", "TvShows/{tvshowId}", new { controller = "TvShowsWeb", action = "Show" });
            routes.MapRoute("TvShowsWeb_GetByName", "TvShows", new { controller = "TvShowsWeb", action = "GetByName" });

            routes.MapRoute(
                "SeasonWeb_Show",
                "TvShows/{tvshowId}/Seasons/{number}",
                new { controller = "SeasonsWeb", action = "Show" });
        }
    }
}