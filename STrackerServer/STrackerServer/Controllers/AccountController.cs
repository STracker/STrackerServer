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
        /// The facebook photo left.
        /// </summary>
        private const string FacebookPhotoLeft = "http://graph.facebook.com/";

        /// <summary>
        /// The facebook photo right.
        /// </summary>
        private const string FacebookPhotoRight = "/picture?type=large";

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
        private string CallbackUri
        {
            get { return "http://" + Request.Url.Host + Url.Action("Callback"); }
        }
        
        /*
        // DEBUG ONLY
        private string CallbackUri
        {
            get
            {
                return Request.Url.GetLeftPart(UriPartial.Authority) + this.Url.Action("Callback");
            }
        }
        */

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

            Response.Cookies.Add(new HttpCookie(StateCookie, new JavaScriptSerializer().Serialize(new CallbackCookie { ReturnUrl = returnUrl, State = state })));
            return new SeeOtherResult { Url = loginUrl.AbsoluteUri };
        }

        /// <summary>
        /// The log out action.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return new SeeOtherResult { Url = Url.Action("Index", "Home") };
        }

        /// <summary>
        /// The callback.
        /// </summary>
        /// <param name="code">
        /// The authorization code.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="error">
        /// The facebook error.
        /// </param>
        /// <param name="error_reason">
        /// The facebook error reason.
        /// </param>
        /// <param name="error_description">
        /// The facebook error description.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Callback(string code, string state, string error, string error_reason, string error_description)
        {
            var cookie = Request.Cookies[StateCookie];

            if (code == null || cookie == null || state == null)
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
                user = new User(me.id) { Email = me.email, Name = me.name, Photo = GetFacebookPhoto(me.id) };
            }
            catch (FacebookOAuthException)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("BadRequest");
            }

            this.usersOperations.VerifyAndSave(user);

            cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie);
            
            FormsAuthentication.SetAuthCookie(user.Id, false);

            return callbackCookie.ReturnUrl == null ? 
                new SeeOtherResult { Url = Url.Action("Index", "Home") } : 
                new SeeOtherResult { Url = callbackCookie.ReturnUrl };
        }

        /// <summary>
        /// The get facebook photo.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetFacebookPhoto(string id)
        {
            return FacebookPhotoLeft + id + FacebookPhotoRight;
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
