// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsRatingsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for television shows ratings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.Hawk;

    /// <summary>
    /// The television shows ratings controller.
    /// </summary>
    public class TvShowsRatingsController : BaseController
    {
        /// <summary>
        /// The operations.
        /// </summary>
        private readonly ITvShowsRatingsOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsRatingsController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        public TvShowsRatingsController(ITvShowsRatingsOperations operations)
        {
            this.operations = operations;
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="id">
        /// The id.
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
        public HttpResponseMessage Post(string id, [FromBody] string rating)
        {
            if (rating == null)
            {
                return this.BasePost(false);
            }

            return this.BasePost(this.operations.AddRating(id, new Rating { UserId = User.Identity.Name, UserRating = int.Parse(rating) }));
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            var ratings = this.operations.Read(id);
            if (ratings == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return this.BaseGet(new { Rating = ratings.Average, Total = ratings.Ratings.Count });
        }
    }
}