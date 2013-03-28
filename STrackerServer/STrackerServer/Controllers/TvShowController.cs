// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Filters;
    using STrackerServer.Testing_Repository;

    /// <summary>
    /// Television show controller.
    /// </summary>
    public class TvShowController : Controller
    {
        /// <summary>
        /// Television shows repository.
        /// </summary>
        private readonly TvShowsTestRepository repository = TestRepositoryLocator.TvShowsTestRepo;

        /// <summary>s
        /// This method is used to show basic information about a television show in JSON format.
        /// </summary>
        /// <param name="tvshowId">
        /// The id is the unique identifier of a television show.
        /// </param>
        /// <returns>
        /// The <see cref="JsonResult"/>.
        /// </returns>
        [TvShowActionFilter]
        public JsonResult JsonGet(int tvshowId)
        {
            return (Response.StatusCode != (int)HttpStatusCode.OK)
                       ? Json(null, JsonRequestBehavior.AllowGet)
                       : Json(repository.Get(tvshowId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method is used to show basic information about a television show in HTML format.
        /// </summary>
        /// <param name="tvshowId">
        /// The id is the unique identifier of a television show.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [TvShowActionFilter]
        public ActionResult Get(int tvshowId)
        {
            return (Response.StatusCode != (int)HttpStatusCode.OK)
                       ? View("Error", Response.StatusCode)
                       : View("Get", repository.Get(tvshowId));
        }

        /// <summary>
        /// This method is used to show basic information about a season of one television show in JSON format.
        /// </summary>
        /// <param name="tvshowId">
        /// The id is the unique identifier of a television show.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <returns>
        /// The <see cref="JsonResult"/>.
        /// </returns>
        [SeasonActionFilter]
        public JsonResult JsonGetSeason(int tvshowId, int seasonNumber)
        {
            object season;
            TempData.TryGetValue("season", out season);
            return (Response.StatusCode != (int)HttpStatusCode.OK)
                       ? Json(null, JsonRequestBehavior.AllowGet)
                       : Json(season, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method is used to show basic information about a season of one television show in HTML format.
        /// </summary>
        /// <param name="tvshowId">
        /// The id is the unique identifier of a television show.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [SeasonActionFilter]
        public ActionResult GetSeason(int tvshowId, int seasonNumber)
        {
            object season;
            TempData.TryGetValue("season", out season);
            return (Response.StatusCode != (int)HttpStatusCode.OK)
                       ? this.View("Error", Response.StatusCode)
                       : this.View("GetSeason", season);
        }
    }
}
