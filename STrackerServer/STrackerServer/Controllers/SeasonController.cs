// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServerDatabase.Core;
    using STrackerServerDatabase.Models;

    /// <summary>
    /// The season controller.
    /// </summary>
    public class SeasonController : Controller
    {
        /// <summary>
        /// Get HTML page with information from the season from one television show.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="number">
        /// The season number.
        /// </param>
        /// <returns>
        /// The <see cref="Season"/>.
        /// </returns>
        public ActionResult Get(string tvshowId, int number)
        {
            var season = RepositoryLocator.SeasonsDocumentRepository.Read(new Tuple<string, int>(tvshowId, number));

            if (season == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;

                return this.View("Error", Response.StatusCode);
            }

            return this.View("Get", season);
        }
    }
}
