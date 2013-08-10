// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserFriendsController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the UserFriendsController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
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
        /// The get info.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [HawkAuthorize]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.usersOperations.Read(User.Identity.Name).Friends);
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="userId">
        /// The user Id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        [HawkAuthorize]
        public HttpResponseMessage Post([FromBody] string userId)
        {
            /*
            if (userId == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Body, Missing required fields.");
            }

            return this.BasePostDelete(this.usersOperations.Invite(User.Identity.Name, userId));*/
            return null;
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpDelete]
        [HawkAuthorize]
        public HttpResponseMessage Delete(string userId)
        {
            return this.BasePostDelete(this.usersOperations.RemoveFriend(User.Identity.Name, userId));
        }
    }
}
