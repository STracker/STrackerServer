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
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.Security;

    using DataAccessLayer.DomainEntities;

    using Facebook;

    using STrackerServer.Action_Results;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// Account controller.
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// The name of the state cookie.
        /// </summary>
        private const string StateCookie = "State";

        /// <summary>
        /// The permissions.
        /// </summary>
        private const string Permissions = "email";

        /// <summary>
        /// The user info query.
        /// </summary>
        private const string UserInfoQuery = "me?fields=id,name,email,picture.type(large)";

        /// <summary>
        /// Facebook client id.
        /// </summary>
        private static readonly string FacebookClientId = ConfigurationManager.AppSettings["Facebook:Key"];

        /// <summary>
        /// Facebook client secret.
        /// </summary>
        private static readonly string FacebookClientSecret = ConfigurationManager.AppSettings["Facebook:Secret"]; 

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
        /*private string CallbackUri
        {
            get { return "http://" + Request.Url.Host + Url.Action("Callback"); }
        }*/
        private string CallbackUri
        {
            get
            {
                var url = this.Request.Url;
                return url == null ? null : url.GetLeftPart(UriPartial.Authority) + this.Url.Action("Callback");
            }
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var fb = new FacebookClient();
            var state = Guid.NewGuid().ToString();
  
            var loginUrl = fb.GetLoginUrl(new
                {
                    client_id = FacebookClientId,
                    redirect_uri = this.CallbackUri,
                    response_type = "code",
                    scope = Permissions,
                    state
                });

            var jsonValue = new JavaScriptSerializer().Serialize(new CallbackCookie
                                                                {
                                                                    ReturnUrl = returnUrl,
                                                                    State = state
                                                                });

            Response.Cookies.Add(new HttpCookie(StateCookie, jsonValue));
            return new SeeOtherResult { Url = loginUrl.AbsoluteUri };
        }

        /// <summary>
        /// The log out action.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return new SeeOtherResult { Url = Url.Action("Index", "HomeWeb") };
        }

        /// <summary>
        /// The callback.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Callback(string code, string state)
        {
            var cookie = Request.Cookies[StateCookie];

            if (code == null || cookie == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.View("Error", (int)HttpStatusCode.Forbidden);
            }

            var callbackCookie = new JavaScriptSerializer().Deserialize<CallbackCookie>(cookie.Value);

            if (!state.Equals(callbackCookie.State)) 
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.View("Error", (int)HttpStatusCode.Forbidden);
            }

            User user;

            try
            {
                var fb = new FacebookClient(); 

                dynamic result = fb.Get(
                    "oauth/access_token",
                    new
                    {
                        client_id = FacebookClientId,
                        client_secret = FacebookClientSecret,
                        redirect_uri = this.CallbackUri,
                        code
                });

                fb.AccessToken = result.access_token;

                dynamic me = fb.Get(UserInfoQuery);
                user = new User(me.id) { Email = me.email, Name = me.name, Photo = new Artwork { ImageUrl = me.picture.data.url } };
            }
            catch (FacebookOAuthException)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", (int)HttpStatusCode.BadRequest);
            }

            this.usersOperations.VerifyAndSave(user);

            cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie);
            
            FormsAuthentication.SetAuthCookie(user.Key, false);

            return callbackCookie.ReturnUrl == null ? 
                new SeeOtherResult { Url = Url.Action("Index", "HomeWeb") } : 
                new SeeOtherResult { Url = callbackCookie.ReturnUrl };
        }

        /// <summary>
        /// The callback cookie.
        /// </summary>
        internal class CallbackCookie
        {
            /// <summary>
            /// Gets or sets the return url.
            /// </summary>
            public string ReturnUrl { get; set; }

            /// <summary>
            /// Gets or sets the state.
            /// </summary>
            public string State { get; set; }
        }
    }
}
