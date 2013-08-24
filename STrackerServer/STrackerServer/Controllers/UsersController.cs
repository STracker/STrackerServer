// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Users web Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Action_Results;
    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Permissions;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Models.Admin;
    using STrackerServer.Models.User;

    /// <summary>
    /// The users web controller.
    /// </summary>
    [Authorize]
    public class UsersController : Controller
    {
        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// The permission manager.
        /// </summary>
        private readonly IPermissionManager<Permissions, int> permissionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        /// <param name="permissionManager">
        /// The permission Manager.
        /// </param>
        public UsersController(IUsersOperations usersOperations, IPermissionManager<Permissions, int> permissionManager)
        {
            this.usersOperations = usersOperations;
            this.permissionManager = permissionManager;
        }

        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Index(string id)
        {
            if (User.Identity.Name.Equals(id))
            {
                return new SeeOtherResult { Url = Url.Action("Index", "User") };
            }

            var user = this.usersOperations.Read(id);

            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("NotFound");
            }

            var currentUser = this.usersOperations.Read(User.Identity.Name);
            var isFriend = user.Friends.Any(synopsis => synopsis.Id.Equals(this.User.Identity.Name)) || user.FriendRequests.Any(synopsis => synopsis.Id.Equals(this.User.Identity.Name));
            var adminMode = this.permissionManager.HasPermission(Permissions.Admin, currentUser.Permission);

            return this.View(new UserPublicView
            {
                Id = id,
                Name = user.Name,
                PictureUrl = user.Photo,
                SubscriptionList = user.Subscriptions,
                IsFriend = isFriend,
                IsAdmin = this.permissionManager.HasPermission(Permissions.Admin, user.Permission),
                AdminMode = adminMode
            });
        }

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Search(string name)
        {
            if (name == null || string.Empty.Equals(name.Trim()))
            {
                return this.View(new UserSearchResult { Result = new List<User.UserSynopsis>(), SearchValue = string.Empty });
            }

            var users = this.usersOperations.ReadByName(name).ToList();

            if (users.Count != 0)
            {
                var index = users.FindIndex(user => user.Id.Equals(User.Identity.Name));
                if (index != -1)
                {
                    users.RemoveAt(index);
                }
            }

            return this.View(new UserSearchResult
            {
                Result = users,
                SearchValue = name
            });
        }

        /// <summary>
        /// The public friends.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Friends(string id)
        {
            if (User.Identity.Name.Equals(id))
            {
                return new SeeOtherResult { Url = Url.Action("Friends", "User") };
            }

            var user = this.usersOperations.Read(id);

            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("NotFound");
            }

            return this.View(new PublicFriendsView
            {
                Id = id,
                Name = user.Name,
                List = user.Friends,
                PictureUrl = user.Photo
            });
        }

        /// <summary>
        /// The view with a form to set the specific user permission
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [STrackerAuthorize(Permission = Permissions.Admin)]
        public ActionResult Permission(string id)
        {
            var user = this.usersOperations.Read(id);

            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("NotFound");
            }

            if (!id.Equals(User.Identity.Name) && (Permissions)user.Permission == Permissions.Admin)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.View("Error", Response.StatusCode);
            }

            return this.View(new SetPermissionView
            {
                Id = user.Id,
                Name = user.Name,
                PictureUrl = user.Photo,
                PermissionName = Enum.GetName(typeof(Permissions), user.Permission),
                Permission = user.Permission,
                Permissions = this.permissionManager.GetPermissions()
            });
        }

        /// <summary>
        /// Set user permission
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [STrackerAuthorize(Permission = Permissions.Admin)]
        public ActionResult Permission(SetPermissionView values)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View(values);
            }

            var user = this.usersOperations.Read(values.Id);

            if (!this.usersOperations.SetUserPermission(values.Id, values.Permission))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("BadRequest");
            }

            if ((User.Identity.Name.Equals(values.Id) && values.Permission < (int)Permissions.Admin) || (!User.Identity.Name.Equals(values.Id) && values.Permission == (int)Permissions.Admin))
            {
                return new SeeOtherResult { Url = Url.Action("Index", new { id = user.Id }) };
            }

            return new SeeOtherResult { Url = Url.Action("Permission", new { id = user.Id }) };
        }
    }
}