// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The user controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Web.Mvc;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Permissions;
    using STrackerServer.Models.User;

    /// <summary>
    /// The user web controller.
    /// </summary>
    public class UserController : Controller
    {
        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// The permission manager.
        /// </summary>
        private readonly IPermissionManager<Permission, int> permissionManager;

        /// <summary>
        /// The new episodes operations.
        /// </summary>
        private readonly INewEpisodesOperations newEpisodesOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        /// <param name="tvshowsOperations">
        /// The television shows operations. 
        /// </param>
        /// <param name="permissionManager">
        /// The permission Manager.
        /// </param>
        /// <param name="newEpisodesOperations">
        /// The new Episodes Operations.
        /// </param>
        public UserController(IUsersOperations usersOperations, ITvShowsOperations tvshowsOperations, IPermissionManager<Permission, int> permissionManager, INewEpisodesOperations newEpisodesOperations)
        {
            this.usersOperations = usersOperations;
            this.tvshowsOperations = tvshowsOperations;
            this.permissionManager = permissionManager;
            this.newEpisodesOperations = newEpisodesOperations;
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var user = this.usersOperations.Read(User.Identity.Name);

            return this.View(new UserPrivateView
            {
                Id = User.Identity.Name,
                Name = user.Name,
                PictureUrl = user.Photo,
                SubscriptionList = user.SubscriptionList,
                IsAdmin = this.permissionManager.HasPermission(Permission.Admin, user.Permission),
                NewEpisodes = this.newEpisodesOperations.GetUserNewEpisodes(User.Identity.Name)
            });
        }
    }
}