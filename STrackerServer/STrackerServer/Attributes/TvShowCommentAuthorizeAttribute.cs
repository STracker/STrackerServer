// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowCommentAuthorizeAttribute.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The comment authorize attribute, verifies if the user is has the permissions
//   or is the owner of the television show comment.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Attributes
{
    using System.Web;
    using System.Web.Mvc;

    using Ninject;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Permissions;
    using STrackerServer.NinjectDependencies;

    /// <summary>
    /// The comment authorize attribute, verifies if the user is has the permissions
    /// or is the owner of the comment.
    /// </summary>
    public class TvShowCommentAuthorizeAttribute : AuthorizeAttribute
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
        private readonly ITvShowsCommentsOperations commentsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="STrackerAuthorizeAttribute"/> class.
        /// </summary>
        public TvShowCommentAuthorizeAttribute()
        {
            IKernel kernel = new StandardKernel(new ModuleForSTracker());
            
            this.manager = kernel.Get<IPermissionManager<Permissions, int>>();
            this.usersOperations = kernel.Get<IUsersOperations>();
            this.commentsOperations = kernel.Get<ITvShowsCommentsOperations>();
        }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        public Permissions Permissions { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates that the user 
        /// can have the permissions or be owner of the resource.
        /// </summary>
        public bool Owner { get; set; }

        /// <summary>
        /// Verifies if the user is has the permissions
        /// or is the owner of the comment.
        /// </summary>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!base.AuthorizeCore(httpContext))
            {
                return false;
            }

            var user = this.usersOperations.Read(httpContext.User.Identity.Name);
            var id = httpContext.Request.RequestContext.RouteData.Values["id"];

            var comments = this.commentsOperations.Read((string)httpContext.Request.RequestContext.RouteData.Values["tvshowId"]);

            // Ignore and let the action responde correctly.
            if (comments == null)
            {
                return true;
            }

            var comment = comments.Comments.Find(com => com.Id.Equals(id));

            // Ignore and let the action responde correctly.
            if (comment == null)
            {
                return true;
            }

            var isOwner = comment.User.Id.Equals(httpContext.User.Identity.Name);

            return this.Owner && isOwner || this.manager.HasPermission(this.Permissions, user.Permission);
        }
    }
}