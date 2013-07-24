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

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Hawk;

    /// <summary>
    /// The new episodes controller.
    /// </summary>
    public class NewEpisodesController : BaseController
    {
        /// <summary>
        /// The episodes operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewEpisodesController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The episodes operations.
        /// </param>
        public NewEpisodesController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
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
            return this.BaseGet(this.usersOperations.GetNewestEpisodes(User.Identity.Name));
        }
    }
}
