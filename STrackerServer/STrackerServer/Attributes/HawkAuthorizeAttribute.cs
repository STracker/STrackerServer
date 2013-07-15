// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HawkAuthorizeAttribute.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the HawkAuthorizeAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Hawk
{
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Security;
    using System.Security.Principal;
    using System.Threading;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using HawkNet;

    using Ninject;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.NinjectDependencies;

    /// <summary>
    /// The hawk authorize attribute.
    /// </summary>
    public class HawkAuthorizeAttribute : AuthorizationFilterAttribute  
    {
        /// <summary>
        /// The scheme.
        /// </summary>
        private const string Scheme = "Hawk";

        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="HawkAuthorizeAttribute"/> class.
        /// </summary>
        public HawkAuthorizeAttribute()
        {
            var kernel = new StandardKernel(new ModuleForSTracker());
            this.usersOperations = kernel.Get<IUsersOperations>();
            this.CheckId = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether check id, default true.
        /// </summary>
        public bool CheckId { get; set; }

        /// <summary>
        /// The on authorization.
        /// </summary>
        /// <param name="actionContext">
        /// The action context.
        /// </param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var request = actionContext.Request;

            if (request.Headers.Authorization == null || string.IsNullOrWhiteSpace(request.Headers.Authorization.Scheme))
            {
                HandleResponse("Authorization header not found", HttpStatusCode.Unauthorized);
                return;
            }

            if (!string.Equals(request.Headers.Authorization.Scheme, Scheme))
            {
                HandleResponse(string.Format("Invalid Schema found {0}", request.Headers.Authorization.Scheme), HttpStatusCode.Unauthorized);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.Headers.Authorization.Parameter))
                {
                    HandleResponse("Invalid header format", HttpStatusCode.BadRequest);
                    return;
                }

                if (string.IsNullOrWhiteSpace(request.Headers.Host))
                {
                    HandleResponse("Missing Host header", HttpStatusCode.BadRequest);
                    return;
                }

                IPrincipal principal;

                try
                {
                    principal = request.Authenticate(GetCredentials);

                    if (this.CheckId && this.usersOperations.Read(principal.Identity.Name) == null)
                    {
                        HandleResponse("Invalid Id", HttpStatusCode.Unauthorized);
                        return;
                    }
                }
                catch (SecurityException ex)
                {
                    HandleResponse(ex.Message, HttpStatusCode.Unauthorized);
                    return;
                }

                Thread.CurrentPrincipal = principal;
            }
        }

        /// <summary>
        /// The handle response.
        /// </summary>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <exception cref="HttpResponseException">
        /// Unauthorized Request Response
        /// </exception>
        private static void HandleResponse(string body, HttpStatusCode status)
        {
            var response = new HttpResponseMessage(status)
            {
                Content = new StringContent(body)
            };
            throw new HttpResponseException(response);
        }

        /// <summary>
        /// The get credentials.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="HawkCredential"/>.
        /// </returns>
        private static HawkCredential GetCredentials(string id)
        {
            return new HawkCredential
                {
                    Id = id,
                    Key = ConfigurationManager.AppSettings["Hawk:Key"],
                    Algorithm = ConfigurationManager.AppSettings["Hawk:Algorithm"], 
                    User = id 
                };
        }
    }
}