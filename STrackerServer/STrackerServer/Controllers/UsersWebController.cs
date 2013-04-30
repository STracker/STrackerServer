// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersWebController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Account Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Net;

namespace STrackerServer.Controllers
{
    using System.Web.Mvc;
    using BusinessLayer.Core;

    using STrackerServer.Models.User;

    /// <summary>
    /// The users controller.
    /// </summary>
    [Authorize]
    public class UsersWebController : Controller
    {
        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersWebController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        public UsersWebController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// The user profile.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public new ActionResult Profile()
        {
            var user = this.usersOperations.Read(User.Identity.Name);

            // Isto é possivel??
            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return this.View("Error");
            }

            // ------------//
            return this.View("Profile", new UserView { Email = user.Key, Name = user.Name });
        }
    }
}
