// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserFriendRequestsController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Api controller for user's friend requests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutUsers_Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Controllers.Api.AuxiliaryObjects;

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
        /// Get all user's friend requests.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [HawkAuthorize]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.usersOperations.Read(this.User.Identity.Name).FriendRequests);
        }

        /// <summary>
        /// Accept one user friend request.
        /// </summary>
        /// <param name="userId">
        /// The user Id that send the request.
        /// </param>
        /// <param name="accept">
        /// The value, reject or accept the request.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        [HawkAuthorize]
        public HttpResponseMessage Post(string userId, [FromBody] ApiRequestResponse accept)
        {
            // Test if null instead of the use model state, because the Accept field is primitive type.
            if (accept == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Body, Missing required fields.");
            }

            if (accept.Accept)
            {
                return this.BasePostDelete(this.usersOperations.AcceptInvite(userId, this.User.Identity.Name));
            }
            
            return this.BasePostDelete(this.usersOperations.RejectInvite(userId, this.User.Identity.Name));
        }
    }
}