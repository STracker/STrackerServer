// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserFriendRequestsController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The user friend requests controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Controllers.Api.AuxiliaryObjects;
    using STrackerServer.Hawk;

    /// <summary>
    /// The user friend requests controller.
    /// </summary>
    public class UserFriendRequestsController : BaseController
    {
                /// <summary>
        /// The operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFriendRequestsController"/> class. 
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        public UserFriendRequestsController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// The get info.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [HawkAuthorize]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.usersOperations.Read(User.Identity.Name).FriendRequests);
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="userId">
        /// The user Id.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        [HawkAuthorize]
        public HttpResponseMessage Post(string userId, [FromBody] ApiRequestResponse response)
        {
            if (!ModelState.IsValid)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Body, Missing required fields.");
            }

            if (response.Accept)
            {
                return this.BasePostDelete(this.usersOperations.AcceptInvite(userId, User.Identity.Name));
            }
            
            return this.BasePostDelete(this.usersOperations.RejectInvite(userId, User.Identity.Name));
        }
    }
}
