// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Core;

    /// <summary>
    /// The television shows controller.
    /// </summary>
    public class TvShowController : BaseController
    {
        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Get(string id)
        {
            var representation = Representations[(string)RouteData.Values["format"]];

            var tvshow = RepositoryLocator.TelevisionShowsRepository.Read(id);

            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return representation.Make(Response.StatusCode, "Error");
            }

            return representation.Make(tvshow, "Get");
        }
    }
}
