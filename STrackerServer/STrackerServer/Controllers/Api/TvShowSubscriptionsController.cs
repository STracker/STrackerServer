﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowSubscriptionsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The tv show subscription controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Hawk;

    /// <summary>
    /// The television show subscription controller.
    /// </summary>
    public class TvShowSubscriptionsController : BaseController
    {
        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowSubscriptionsController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        public TvShowSubscriptionsController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        [HawkAuthorize]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.usersOperations.Read(User.Identity.Name).SubscriptionList);
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        [HawkAuthorize]
        public HttpResponseMessage Post(string tvshowId)
        {
            return this.BasePostDelete(this.usersOperations.AddSubscription(User.Identity.Name, tvshowId));
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpDelete]
        [HawkAuthorize]
        public HttpResponseMessage Delete(string tvshowId)
        {
            return this.BasePostDelete(this.usersOperations.RemoveSubscription(User.Identity.Name, tvshowId));
        }
    }
}
