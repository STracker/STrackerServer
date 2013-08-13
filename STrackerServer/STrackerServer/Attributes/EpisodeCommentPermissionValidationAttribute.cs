// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeCommentPermissionValidationAttribute.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The comment authorize attribute, verifies if the user is has the permissions
//   or is the owner of the episode comment.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Attributes
{
    using System.Net;
    using System.Web.Mvc;

    using Ninject;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Permissions;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.NinjectDependencies;

    /// <summary>
    /// The comment authorize attribute, verifies if the user is has the permissions
    /// or is the owner of the comment.
    /// </summary>
    public class EpisodeCommentPermissionValidationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The manager.
        /// </summary>
        private readonly IPermissionManager<Permissions, int> manager;

        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// The comments operations.
        /// </summary>
        private readonly IEpisodesCommentsOperations commentsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeCommentPermissionValidationAttribute"/> class. 
        /// </summary>
        public EpisodeCommentPermissionValidationAttribute()
        {
            IKernel kernel = new StandardKernel(new ModuleForSTracker());

            this.manager = kernel.Get<IPermissionManager<Permissions, int>>();
            this.usersOperations = kernel.Get<IUsersOperations>();
            this.commentsOperations = kernel.Get<IEpisodesCommentsOperations>();
        }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        public Permissions Permissions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether that the user 
        /// can have the permissions or be owner of the resource.
        /// </summary>
        public bool Owner { get; set; }

        /// <summary>
        /// Action executing.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = this.usersOperations.Read(filterContext.HttpContext.User.Identity.Name);
            var id = filterContext.HttpContext.Request.RequestContext.RouteData.Values["id"];

            var key = new Episode.EpisodeId
            {
                TvShowId = (string)filterContext.HttpContext.Request.RequestContext.RouteData.Values["tvshowId"],
                SeasonNumber = int.Parse((string)filterContext.HttpContext.Request.RequestContext.RouteData.Values["seasonNumber"]),
                EpisodeNumber = int.Parse((string)filterContext.HttpContext.Request.RequestContext.RouteData.Values["episodeNumber"])
            };

            var comments = this.commentsOperations.Read(key);

            // Ignore and let the action responde correctly.
            if (comments == null)
            {
                return;
            }

            var comment = comments.Comments.Find(com => com.Id.Equals(id));

            // Ignore and let the action responde correctly.
            if (comment == null)
            {
                return;
            }

            var isOwner = comment.User.Id.Equals(filterContext.HttpContext.User.Identity.Name);

            if ((this.Owner && isOwner) || this.manager.HasPermission(this.Permissions, user.Permission))
            {
                return;
            }

            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            filterContext.Result = new ViewResult
            {
                ViewName = @"~/Views/Shared/Error.cshtml",
                ViewData = new ViewDataDictionary((int)HttpStatusCode.Forbidden)
            };
        }
    }
}