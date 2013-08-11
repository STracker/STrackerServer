// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserFriendsController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Api controller for user's friends.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutUsers_Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.UsersOperations;

    /// <summary>
    /// The user friends controller.
    /// </summary>
    public class UserFriendsController : BaseController
    {
        /// <summary>
        /// The operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFriendsController"/> class. 
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        public UserFriendsController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// Get a list with all user's friends.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [HawkAuthorize]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.usersOperations.Read(this.User.Identity.Name).Friends);
        }

        /// <summary>
        /// Invite one user to user's friend list.
        /// </summary>
        /// <param name="userId">
        /// The user Id to invite.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        [HawkAuthorize]
        public HttpResponseMessage Post([FromBody] string userId)
        {
            return userId == null ? this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Body, Missing required fields.") 
                                  : this.BasePostDelete(this.usersOperations.InviteFriend(this.User.Identity.Name, userId));
        }

        /// <summary>
        /// Delete one friend from user's list.
        /// </summary>
        /// <param name="userId">
        /// The user id of the friend for remove.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpDelete]
        [HawkAuthorize]
        public HttpResponseMessage Delete(string userId)
        {
            return this.BasePostDelete(this.usersOperations.RemoveFriend(this.User.Identity.Name, userId));
        }
    }
}