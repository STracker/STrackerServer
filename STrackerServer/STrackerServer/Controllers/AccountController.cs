﻿// --------------------------------------------------------------------------------------------------------------------
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
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.Security;
 
    using BusinessLayer.Core;
    using Custom_action_results;
    using DataAccessLayer.DomainEntities;

    using Facebook;

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
        private string CallbackUri
        {
            get { return Request.Url.GetLeftPart(UriPartial.Authority) + Url.Action("Callback"); }
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
            var encoding = new ASCIIEncoding();
            var state = Guid.NewGuid().ToString();
  
            var loginUrl = fb.GetLoginUrl(new
                {
                    client_id = FacebookClientId,
                    redirect_uri = this.CallbackUri,
                    response_type = "code",
                    scope = Permissions,
                    state = MD5.Create().ComputeHash(encoding.GetBytes(state))
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

            if (cookie == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.View("Error");
            }

            var callbackCookie = new JavaScriptSerializer().Deserialize<CallbackCookie>(cookie.Value);

            var encoding = new ASCIIEncoding();

            if (state.Equals(MD5.Create().ComputeHash(encoding.GetBytes(callbackCookie.State)).ToString())) 
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.View("Error");
            }

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
                        redirect_uri = this.CallbackUri,
                        code
                });

                fb.AccessToken = result.access_token;

                dynamic me = fb.Get("me?fields=name,email,picture");

                user = new User(me.email) { Name = me.name, Photo = new Artwork { ImageUrl = me.picture.data.url } };
            }
            catch (Exception /*or only FacebookOAuthException???*/)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error");
            }
            
            // False - Error while trying to update
            if (!this.usersOperations.VerifyAndSave(user))
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return this.View("Error");
            }

            cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SetAuthCookie(user.Key, false);

            return callbackCookie.ReturnUrl == null ? new SeeOtherResult { Url = Url.Action("Index","HomeWeb")} : new SeeOtherResult { Url = callbackCookie.ReturnUrl };
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
