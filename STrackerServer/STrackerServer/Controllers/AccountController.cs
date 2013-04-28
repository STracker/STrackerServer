// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Account Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace STrackerServer.Controllers
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
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
        /// Facebook client id.
        /// </summary>
        private static readonly string FacebookClientId = ConfigurationManager.AppSettings["Client:Id"];

        /// <summary>
        /// Facebook client secret.
        /// </summary>
        private static readonly string FacebookClientSecret = ConfigurationManager.AppSettings["Client:Secret"];

        /// <summary>
        /// The name of the state cookie.
        /// </summary>
        private const string stateCookie = "State";

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

        [HttpGet]
        public ActionResult Login(string returnUri)
        {
            var fb = new FacebookClient();
            var encoding = new ASCIIEncoding();
            var state = Guid.NewGuid().ToString();
  
            var loginUrl = fb.GetLoginUrl(new
                {
                    client_id = FacebookClientId,
                    redirect_uri = this.CallbackUri,
                    response_type = "code",
                    scope = "email,publish_actions",
                    state = MD5.Create().ComputeHash(encoding.GetBytes(state))
                });

            Response.Cookies.Add(new HttpCookie(stateCookie, state));
            return new SeeOtherResult { Uri = loginUrl.AbsoluteUri };
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
            return new SeeOtherResult { Uri = Url.Action("Index", "HomeWeb") };
        }

        [HttpGet]
        public ActionResult Callback(string code, string state)
        {
            var cookie = Request.Cookies[stateCookie];

            if (cookie == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.View("Error");
            }

            var encoding = new ASCIIEncoding();

            if (state.Equals(MD5.Create().ComputeHash(encoding.GetBytes(cookie.Value)).ToString())) 
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
                
                // TODO, create view for this error. 
                return this.View("Error");
            }
            
            // False - Error while trying to update
            if (!this.usersOperations.VerifyAndSave(user))
            {
                // TODO, create view for this error. 
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return this.View("Error");
            }

            Response.Cookies.Remove(stateCookie);
            FormsAuthentication.SetAuthCookie(user.Key, false);
            return new SeeOtherResult { Uri = Url.Action("Index", "HomeWeb") };
        }
    }
}
