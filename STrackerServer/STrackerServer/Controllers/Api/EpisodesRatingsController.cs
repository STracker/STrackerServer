// --------------------------------------------------------------------------------------------------------------------
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
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.Controllers.Api.AuxiliaryObjects;
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
        /// Initializes a new instance of the <see cref="EpisodesRatingsController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        public EpisodesRatingsController(IEpisodesRatingsOperations operations)
        {
            this.operations = operations;
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
            return this.BaseGet(this.operations.GetAllRatings(new Tuple<string, int, int>(tvshowId, seasonNumber, number)));
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
        [HttpPost]
        public HttpResponseMessage Post(string tvshowId, int seasonNumber, int number, [FromBody] ApiAddRating rating)
        {
            if (!ModelState.IsValid)
            {
                return this.BasePost(false);
            }

            return this.BasePost(this.operations.AddRating(new Tuple<string, int, int>(tvshowId, seasonNumber, number), new Rating { UserId = rating.UserId, UserRating = int.Parse(rating.UserRating) }));
        }
    }
}
