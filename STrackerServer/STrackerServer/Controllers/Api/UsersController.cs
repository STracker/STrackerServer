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
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Controllers.Api.AuxiliaryObjects;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
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
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [HawkAuthorize]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.operations.Read(User.Identity.Name));
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="register">
        /// The register.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        [HawkAuthorize(CheckId = false)]
        public HttpResponseMessage Post([FromBody] ApiRegister register)
        {
            if (!ModelState.IsValid)
            {
                this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Body, missing required fields.");
            }

            var user = new User(register.Id)
                {
                    Name = register.Name, 
                    Email = register.Email, 
                    Photo = register.Photo
                };

            this.operations.VerifyAndSave(user);
            return this.BaseGet(this.operations.Read(register.Id));
        }
    }
}