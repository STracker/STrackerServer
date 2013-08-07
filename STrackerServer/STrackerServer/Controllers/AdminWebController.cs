// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdminWebController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The admin web controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Action_Results;
    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.AdminOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Permissions;
    using STrackerServer.Models.Admin;

    /// <summary>
    /// The admin web controller.
    /// </summary>
    public class AdminWebController : Controller
    {
        /// <summary>
        /// The admin operations.
        /// </summary>
        private readonly IAdminOperations adminOperations;

        /// <summary>
        /// The permission manager.
        /// </summary>
        private readonly IPermissionManager<Permission, int> permissionManager;

        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminWebController"/> class.
        /// </summary>
        /// <param name="permissionManager">
        /// The permission manager.
        /// </param>
        /// <param name="adminOperations">
        /// The admin operations.
        /// </param>
        /// <param name="usersOperations">
        /// The users Operations.
        /// </param>
        public AdminWebController(IPermissionManager<Permission, int> permissionManager, IAdminOperations adminOperations, IUsersOperations usersOperations)
        {
            this.adminOperations = adminOperations;
            this.permissionManager = permissionManager;
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// The set user permission.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [STrackerAuthorize(Permission = Permission.Admin)]
        public ActionResult SetUserPermission(string id)
        {
            var user = this.usersOperations.Read(id);

            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            if (!id.Equals(User.Identity.Name) && (Permission)user.Permission == Permission.Admin)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.View("Error", Response.StatusCode);
            }

            return this.View(new SetPermissionView
                {
                    Id = user.Key,
                    Name = user.Name,
                    PictureUrl = user.Photo,
                    PermissionName = Enum.GetName(typeof(Permission), user.Permission),
                    Permission = user.Permission,
                    Permissions = this.permissionManager.GetPermissions()
                });   
        }

        /// <summary>
        /// The create television show.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [STrackerAuthorize(Permission = Permission.Admin)]
        public ActionResult SetUserPermission(SetPermissionView values)
        {
            var user = this.usersOperations.Read(values.Id);

            if (user != null)
            {
                values.Name = user.Name;
                values.PermissionName = Enum.GetName(typeof(Permission), user.Permission);
                values.PictureUrl = user.Photo;
                values.Permissions = this.permissionManager.GetPermissions();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", this.Response.StatusCode);
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View(values);
            }

            if (!this.adminOperations.SetUserPermission(User.Identity.Name, values.Id, this.permissionManager.GetPermission(values.Permission)))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View(values);
            }

            if ((User.Identity.Name.Equals(values.Id) && values.Permission < (int)Permission.Admin) || (!User.Identity.Name.Equals(values.Id) && values.Permission == (int)Permission.Admin))
            {
                return new SeeOtherResult { Url = Url.Action("Show", "UsersWeb", new { id = user.Key }) };
            }

            return new SeeOtherResult { Url = Url.Action("SetUserPermission", "AdminWeb", new { id = user.Key }) };
        }
    }
}
