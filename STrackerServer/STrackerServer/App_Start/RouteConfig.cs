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

            routes.MapRoute("UserView", "User/{action}", new { controller = "UsersWeb" });

            routes.MapRoute("Account_Login", "Account/Login", new { controller = "Account", action = "Login", returnUrl = UrlParameter.Optional });
            routes.MapRoute("Account_Callback", "Account/Callback", new { controller = "Account", action = "Callback" });
            routes.MapRoute("Account_Logout", "Account/Logout", new { controller = "Account", action = "Logout" });

            routes.MapRoute("TvShowsWeb_Show", "TvShows/{tvshowId}", new { controller = "TvShowsWeb", action = "Show" });
            routes.MapRoute("TvShowsWeb_GetByName", "TvShows", new { controller = "TvShowsWeb", action = "GetByName" });

            routes.MapRoute("SeasonWeb_Show", "TvShows/{tvshowId}/Seasons/{number}", new { controller = "SeasonsWeb", action = "Show" });
        }
    }
}