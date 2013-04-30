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
        /// Get one television show by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult GetByName(string name)
        {
            OperationResultState state;
            var tvshow = this.tvshowOps.TryReadByName(name, out state);

            switch (state)
            {
                    case OperationResultState.Completed:
                    return this.View("Show", new TvShowWeb(tvshow));

                    case OperationResultState.InProcess:
                    return this.View("Error", (int)HttpStatusCode.Accepted);

                    case OperationResultState.NotFound:
                    return this.View("Error", (int)HttpStatusCode.NotFound);

                    default:
                    return this.View("Error", (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}