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
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.Hawk;

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
            var ratings = this.operations.GetAllRatings(new Tuple<string, int, int>(tvshowId, seasonNumber, number));
            if (ratings == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return this.BaseGet(ratings.Ratings);
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
        public HttpResponseMessage Post(string tvshowId, int seasonNumber, int number, [FromBody][Required] string rating)
        {
            if (!ModelState.IsValid)
            {
                return this.BasePost(false);
            }

            return this.BasePost(this.operations.AddRating(new Tuple<string, int, int>(tvshowId, seasonNumber, number), new Rating { UserId = User.Identity.Name, UserRating = int.Parse(rating) }));
        }
    }
}
