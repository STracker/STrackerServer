// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserSubscriptionsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api controller for user's subscriptions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutUsers_Controllers
{
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.UsersOperations;

    /// <summary>
    /// The television show subscription controller.
    /// </summary>
    public class UserSubscriptionsController : BaseController
    {
        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscriptionsController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        public UserSubscriptionsController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// Get all user's subscriptions.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [HawkAuthorize]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.usersOperations.Read(this.User.Identity.Name).Subscriptions);
        }

        /// <summary>
        /// Create one subscription to user's subscriptions list.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id to add to subscriptions list.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        [HawkAuthorize]
        public HttpResponseMessage Post([FromBody] string tvshowId)
        {
            if (tvshowId == null)
            {
                return this.BasePostDelete(false);
            }

            return this.BasePostDelete(this.usersOperations.AddSubscription(this.User.Identity.Name, tvshowId));
        }

        /// <summary>
        /// Remove one subscription from user's subscriptions list.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id for remove.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpDelete]
        [HawkAuthorize]
        public HttpResponseMessage Delete(string tvshowId)
        {
            return this.BasePostDelete(this.usersOperations.RemoveSubscription(this.User.Identity.Name, tvshowId));
        }
    }
}