// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Account Api Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System;
    using System.Net.Http;
    using System.Web.Http;

    using HawkNet.WebApi;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Hawk;

    /// <summary>
    /// The users controller.
    /// </summary>
    public class UsersController : BaseController
    {
        /// <summary>
        /// The operations.
        /// </summary>
        private readonly IUsersOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        public UsersController(IUsersOperations usersOperations)
        {
            this.operations = usersOperations;
        }

        /// <summary>
        /// The get info.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public HttpResponseMessage Get(string userId)
        {
            return this.BaseGet(this.operations.Read(userId));
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Post()
        {
            throw new NotImplementedException();
        }
    }
}