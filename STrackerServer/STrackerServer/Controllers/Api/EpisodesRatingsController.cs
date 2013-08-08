﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesRatingsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for episodes ratings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The episodes ratings controller.
    /// </summary>
    public class EpisodesRatingsController : BaseController
    {
        /// <summary>
        /// The operations.
        /// </summary>
        private readonly IEpisodesRatingsOperations operations;

        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesRatingsController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        /// <param name="usersOperations">
        /// The users Operations.
        /// </param>
        public EpisodesRatingsController(IEpisodesRatingsOperations operations, IUsersOperations usersOperations)
        {
            this.operations = operations;
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// The get.
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
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get(string tvshowId, int seasonNumber, int number)
        {
            var ratings = this.operations.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, number));
            if (ratings == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return this.BaseGet(new { Rating = (int)ratings.Average, Total = ratings.Ratings.Count });
        }

        /// <summary>
        /// The post.
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
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        /// The type rating is string because web api validation don't validate value types.
        [HttpPost]
        [HawkAuthorize]
        public HttpResponseMessage Post(string tvshowId, int seasonNumber, int number, [FromBody] int rating)
        {
            return this.BasePostDelete(this.operations.AddRating(new Tuple<string, int, int>(tvshowId, seasonNumber, number), new Rating { User = this.usersOperations.Read(User.Identity.Name).GetSynopsis(), UserRating = rating }));
        }
    }
}
