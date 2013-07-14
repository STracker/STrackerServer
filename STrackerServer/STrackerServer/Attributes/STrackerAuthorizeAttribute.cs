// --------------------------------------------------------------------------------------------------------------------
// <copyright file="STrackerAuthorizeAttribute.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The s tracker authorize attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Attributes
{
    using System.Web;
    using System.Web.Mvc;

    using Ninject;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Permissions;
    using STrackerServer.NinjectDependencies;

    /// <summary>
    /// The s tracker authorize attribute.
    /// </summary>
    public class STrackerAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// The manager.
        /// </summary>
        private readonly IPermissionManager<int> manager;

        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="STrackerAuthorizeAttribute"/> class.
        /// </summary>
        public STrackerAuthorizeAttribute()
        {
            using (IKernel kernel = new StandardKernel(new ModuleForSTracker()))
            {
                this.manager = kernel.Get<IPermissionManager<int>>();
                this.usersOperations = kernel.Get<IUsersOperations>();
            }
        }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        public Permission Permission { get; set; }

        /// <summary>
        /// The authorize core.
        /// </summary>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = this.usersOperations.Read(httpContext.User.Identity.Name);
            return base.AuthorizeCore(httpContext) && this.manager.HasPermission((int)this.Permission, user.Permission);
        }
    }
}