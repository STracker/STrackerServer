// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Controllers
{
    using System.Web.Mvc;

    using STrackerServer.Filters;

    /// <summary>
    /// Season controller.
    /// </summary>
    public class SeasonController : BaseController
    {
        /// <summary>
        /// Get basic information about one season from one television show.
        /// </summary>
        /// <param name="tvshowId">
        /// Television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// Season number.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [SeasonActionFilter]
        public ActionResult Get(string tvshowId, int? seasonNumber)
        {
            return BaseGet("season");
        }
    }
}
