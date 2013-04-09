// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiSeasonController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServerDatabase.Core;
    using STrackerServerDatabase.Models;

    /// <summary>
    /// Seasons API Controller.
    /// </summary>
    public class ApiSeasonController : ApiController
    {
        /// <summary>
        /// Get information from the season from one television show.
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
        public Season Get(string tvshowId, int number)
        {
            var season = RepositoryLocator.SeasonsDocumentRepository.Read(new Tuple<string, int>(tvshowId, number));

            if (season == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            return season;
        }
    }
}
