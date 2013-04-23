// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Account Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Security;

    using Facebook;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.Custom_action_results;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Account controller.
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Facebook client id.
        /// </summary>
        private static readonly string FacebookClientId = ConfigurationManager.AppSettings["Client:Id"];

        /// <summary>
        /// Facebook client secret.
        /// </summary>
        private static readonly string FacebookClientSecret = ConfigurationManager.AppSettings["Client:Secret"];

        /// <summary>
        /// Users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        public AccountController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// Gets the callback uri.
        /// </summary>
        /// Attention! This property must be called when exists one http request.
        private Uri CallbackUri
        {
            get
            {
                var url = Request.Url;
                return (url != null) ? new UriBuilder(url) { Path = Url.Action("Callback") }.Uri : null;
            }
        }

        /// <summary>
        /// Login action.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Login()
        {
            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new
                {
                    client_id = FacebookClientId,
                    redirect_uri = this.CallbackUri.AbsoluteUri,
                    response_type = "code",
                    scope = "email,publish_actions" 
                });

            return new SeeOtherResult { Uri = loginUrl.AbsoluteUri };
        }

        /// <summary>
        /// Callback method. Called after login.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Callback(string code)
        {
            User user;
            try
            {
                var fb = new FacebookClient();

                dynamic result = fb.Post(
                    "oauth/access_token",
                    new
                    {
                        client_id = FacebookClientId,
                        client_secret = FacebookClientSecret,
                        redirect_uri = this.CallbackUri.AbsoluteUri,
                        code
                });

                // Get acess token.
                fb.AccessToken = result.access_token;

                // Creates a dynamic object with desire fields.
                dynamic me = fb.Get("me?fields=name,email,picture");

                user = new User(me.email) { Name = me.name, Photo = new Artwork { ImageUrl = me.picture.data.url } };
            }
            catch (Exception /*or only FacebookOAuthException???*/)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                
                // TODO, create view for this error. 
                return this.View("Error");
            }
            
            if (!this.usersOperations.VerifyAndSave(user))
            {
                // TODO, create view for this error. 
                return this.View("Error");
            }

            FormsAuthentication.SetAuthCookie(user.Key, false);
            return new SeeOtherResult { Uri = Url.Action("GetInfo", "Users") };
        }
    }
}
