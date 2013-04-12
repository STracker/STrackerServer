﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiTvShowController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Mvc;

    using STrackerServerBusinessLayer.Core;

    using STrackerServerDatabase.Core;
    using STrackerServerDatabase.Models;

    /// <summary>
    /// Television shows API controller.
    /// </summary>
    public class ApiTvShowController : ApiController
    {
        private readonly ITvShowsFacade tvShowsFacade;

        public ApiTvShowController(ITvShowsFacade tvShowsFacade)
        {
            this.tvShowsFacade = tvShowsFacade;
        }

        /// <summary>
        /// Get information from the television show with id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public TvShow Get(string id)
        {
            var tvshow = tvShowsFacade.Read(id);

            if (tvshow == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            return tvshow;
        }
    }
}
