// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Users Api Controller.
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
    using STrackerServer.DataAccessLayer.DomainEntities;

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
        /// Get one user information.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [HawkAuthorize]
        public HttpResponseMessage Get(string id)
        {
            return this.BaseGet(this.operations.Read(id));
        }

        /// <summary>
        /// Get users synopsis with the name equals to name in parameters.
        /// </summary>
        /// <param name="name">
        /// The name for search.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [HawkAuthorize]
        public HttpResponseMessage GetByName(string name)
        {
            return this.BaseGet(this.operations.ReadByName(name));
        }

        /// <summary>
        /// Register one user into STracker system.
        /// </summary>
        /// <param name="register">
        /// The register object with all required fields.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        [HawkAuthorize(CheckId = false)]
        public HttpResponseMessage Post([FromBody] ApiRegister register)
        {
            if (register == null || !this.ModelState.IsValid)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Body, missing required fields.");
            }

            this.operations.VerifyAndSave(new User(this.User.Identity.Name)
            {
                Name = register.Name, 
                Email = register.Email, 
                Photo = register.Photo
            });

            return this.BaseGet(this.operations.Read(this.User.Identity.Name));
        }
    }
}