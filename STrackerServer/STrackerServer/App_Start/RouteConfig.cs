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

            routes.MapRoute("User", "User/{action}", new { controller = "UsersWeb" });

            routes.MapRoute("Account_Login", "Account/Login", new { controller = "Account", action = "Login", returnUri = UrlParameter.Optional });
            routes.MapRoute("Account_Callback", "Account/Callback", new { controller = "Account", action = "Callback" });
            routes.MapRoute("Account_Logout", "Account/Logout", new { controller = "Account", action = "Logout" });

            routes.MapRoute("TvShowsWeb_Index", "TvShows", new { controller = "TvShowsWeb", action = "Index" });
            routes.MapRoute("TvShowsWeb_Show", "TvShows/{tvshowId}", new { controller = "TvShowsWeb", action = "Show" });

            routes.MapRoute("SeasonWeb_Show", "TvShows/{tvshowId}/Seasons/{seasonId}", new { controller = "SeasonWeb", action = "Show" });

            routes.MapRoute("SeasonWeb_All", "TvShows/{tvshowId}/Seasons", new { controller = "SeasonWeb", action = "Index" });
        }
    }
}