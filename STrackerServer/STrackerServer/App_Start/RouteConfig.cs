﻿// --------------------------------------------------------------------------------------------------------------------
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
            routes.MapRoute("Contact", "Api", new { controller = "HomeWeb", action = "Api" });

            // User Routes
            routes.MapRoute("User_Requests_Response", "User/Requests/{id}", new { controller = "UsersWeb", action = "RequestResponse" });

            routes.MapRoute("User_Suggestions", "User/Suggestions", new { controller = "UsersWeb", action = "Suggestions" });

            routes.MapRoute("User_Suggestions_TvShow_Remove", "User/Suggestions/{tvshowId}/remove", new { controller = "UsersWeb", action = "RemoveTvShowSuggestions" });

            routes.MapRoute("User_Requests", "User/Requests", new { controller = "UsersWeb", action = "Requests" });

            routes.MapRoute("User_Invite", "User/Invite", new { controller = "UsersWeb", action = "Invite" });

            routes.MapRoute("User_Friends_Public", "User/{id}/Friends", new { controller = "UsersWeb", action = "PublicFriends" });

            routes.MapRoute("User_Friends_Remove", "User/Friends/{friendId}/Remove", new { controller = "UsersWeb", action = "RemoveFriend" });

            routes.MapRoute("User_Friends", "User/Friends", new { controller = "UsersWeb", action = "Friends" });

            routes.MapRoute("User_Search", "User/Search", new { controller = "UsersWeb", action = "GetByName" });

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

            // Genre Routes
            routes.MapRoute(
                "GenreWeb_Show", 
                "Genres/{name}",
                new { controller = "GenreWeb", action = "Show" });
        }
    }
}