// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiEpisodeController.cs" company="STracker">
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
    /// Episodes API controller.
    /// </summary>
    public class ApiEpisodeController : ApiController
    {
        /// <summary>
        /// Get information from the season from one season form one television show.
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
        /// The <see cref="Episode"/>.
        /// </returns>
        public Episode Get(string tvshowId, int seasonNumber, int number)
        {
            return null;
        }
    }
}
