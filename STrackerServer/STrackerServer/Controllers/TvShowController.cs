// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using STrackerServer.Testing_Repository;

    /// <summary>
    /// The television show controller.
    /// </summary>
    public class TvShowController : Controller
    {
        /// <summary>
        /// The Data Repository.
        /// </summary>
        private readonly TvShowsTestRepository repository = TestRepositoryLocator.TvShowsTestRepo;

        /// <summary>
        /// This method is used to show information about a television show.
        /// </summary>
        /// <param name="id">
        /// The id is the unique identifier of a television show.
        /// </param>
        /// <returns>
        /// The <see cref="JsonResult"/>.
        /// </returns>
        public JsonResult ShowJson(int id)
        {
            var tvshow = this.repository.Get(id);
            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null, JsonRequestBehavior.AllowGet);
            }

            return this.Json(tvshow, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method is used to show information about a television show.
        /// </summary>
        /// <param name="id">
        /// The id is the unique identifier of a television show.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Show(int id)
        {
            var tvshow = this.repository.Get(id);
            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            return this.View("Show", tvshow);
        }
    }
}
