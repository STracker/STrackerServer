// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using STrackerServerDatabase.Core;

    /// <summary>
    /// The television shows controller.
    /// </summary>
    public class TvShowController : Controller 
    {
        /// <summary>
        /// Get HTML page with information from the television show with id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Get(string id)
        {
            var tvshow = RepositoryLocator.TelevisionShowsDocumentRepository.Read(id);

            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            return this.View("Get", tvshow);
        }
    }
}
