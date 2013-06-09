// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserSubscriptionsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for user subscriptions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Controllers.Api.AuxiliaryObjects;

    /// <summary>
    /// The user subscriptions controller.
    /// </summary>
    public class UserSubscriptionsController : BaseController
    {
        /// <summary>
        /// The operations.
        /// </summary>
        private readonly IUsersOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscriptionsController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        public UserSubscriptionsController(IUsersOperations operations)
        {
            this.operations = operations;
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
            var user = this.operations.Read(id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return this.BaseGet(user.SubscriptionList);
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Post(string id, [FromBody] ApiAddUserSubscription value)
        {
            if (!ModelState.IsValid)
            {
                return this.BasePostOrDelete(false);
            }

            var state = this.operations.AddSubscription(id, value.TvShowId);
            return this.BasePostOrDelete(state);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpDelete]
        public HttpResponseMessage Delete(string userId, string tvshowId)
        {
            return null;
        }
    }
}