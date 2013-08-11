// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsRatingsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for television shows ratings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutTvShows_Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The television shows ratings controller.
    /// </summary>
    public class TvShowsRatingsController : BaseController
    {
        /// <summary>
        /// Operations object.
        /// </summary>
        private readonly ITvShowsRatingsOperations operations;

        /// <summary>
        /// Users operations object.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsRatingsController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        /// <param name="usersOperations">
        /// The users Operations.
        /// </param>
        public TvShowsRatingsController(ITvShowsRatingsOperations operations, IUsersOperations usersOperations)
        {
            this.operations = operations;
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// Get the rating information about one television show.
        /// </summary>
        /// <param name="id">
        /// The id of the television show.
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

            return this.BaseGet(new { Rating = (int)ratings.Average, Total = ratings.Ratings.Count });
        }

        /// <summary>
        /// Creates one user rating from one television show.
        /// </summary>
        /// <param name="id">
        /// The id of the television show.
        /// </param>
        /// <param name="rating">
        /// The user's rating.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        /// The type rating is string because web api validation don't validate value types.
        [HttpPost]
        [HawkAuthorize]
        public HttpResponseMessage Post(string id, [FromBody] int rating)
        {
            return this.BasePostDelete(this.operations.AddRating(id, new Rating { User = this.usersOperations.Read(this.User.Identity.Name).GetSynopsis(), UserRating = rating }));
        }
    }
}