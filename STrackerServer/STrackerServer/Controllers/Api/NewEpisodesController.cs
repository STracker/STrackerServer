// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewEpisodesController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The new episodes controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.Hawk;

    /// <summary>
    /// The new episodes controller.
    /// </summary>
    public class NewEpisodesController : BaseController
    {
        /// <summary>
        /// The episodes operations.
        /// </summary>
        private readonly INewEpisodesOperations newEpisodesOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewEpisodesController"/> class.
        /// </summary>
        /// <param name="newEpisodesOperations">
        /// The new Episodes Operations.
        /// </param>
        public NewEpisodesController(INewEpisodesOperations newEpisodesOperations)
        {
            this.newEpisodesOperations = newEpisodesOperations;
        }

        /// <summary>
        /// Get the new episodes from user's subscribed television show.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [HawkAuthorize]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.newEpisodesOperations.GetUserNewEpisodes(User.Identity.Name));
        }

        /// <summary>
        /// The get newest episodes.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get(string tvshowId, string date)
        {
            return this.BaseGet(this.newEpisodesOperations.GetNewEpisodes(tvshowId, date));
        }
    }
}
