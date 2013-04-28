// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsWebController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Account Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;
    using BusinessLayer.Core;
    using DataAccessLayer.DomainEntities;
    using Models.TvShow;

    /// <summary>
    /// The television shows web controller.
    /// </summary>
    public class TvShowsWebController : Controller
    {
        /// <summary>
        /// The operations of the Television Show Controller.
        /// </summary>
        private readonly ITvShowsOperations tvshowOps;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsWebController"/> class.
        /// </summary>
        /// <param name="tvshowOps">
        /// The operations.
        /// </param>
        public TvShowsWebController(ITvShowsOperations tvshowOps)
        {
            this.tvshowOps = tvshowOps;
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// The television show
        /// </summary>
        /// <param name="tvshowId">
        /// The television Show Id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Show(string tvshowId)
        {
            TvShow tvshow = this.tvshowOps.Read(tvshowId);

            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error");
            }

            TvShowWeb model = new TvShowWeb(tvshow);

            return this.View(model);
        }

        /// <summary>
        /// The television show web search.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Search(string name)
        {
            return this.View();
        }
    }
}