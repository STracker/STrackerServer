// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServerDatabase.Core;

    /// <summary>
    /// The episode controller.
    /// </summary>
    public class EpisodeController : Controller
    {
        /// <summary>
        /// Get HTML page with information from the episode from one season from one television show.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Get(string tvshowId, int seasonNumber, int number)
       {
           var episode =
               RepositoryLocator.EpisodesDocumentRepository.Read(
                   new Tuple<string, int, int>(tvshowId, seasonNumber, number));

           if (episode == null)
           {
               Response.StatusCode = (int)HttpStatusCode.NotFound;
               return this.View("Error", Response.StatusCode);
           }

           return this.View("Get", episode);
       }
    }
}
