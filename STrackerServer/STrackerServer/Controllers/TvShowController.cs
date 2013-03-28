﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Filters;

    /// <summary>
    /// Television show controller.
    /// </summary>
    public class TvShowController : BaseController
    {
        /// <summary>
        /// Get basic information about a television show.
        /// </summary>
        /// <param name="tvshowId">
        /// The id is the unique identifier of a television show.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [TvShowActionFilter]
        public ActionResult Get(string tvshowId)
        {
            return BaseGet("tvshow");
        }
    }
}
