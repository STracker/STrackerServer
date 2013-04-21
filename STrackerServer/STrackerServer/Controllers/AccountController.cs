using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Facebook;
using STrackerServer.BusinessLayer.Core;
using STrackerServer.DataAccessLayer.DomainEntities;

namespace STrackerServer.Controllers
{
    public class AccountController : Controller
    {
        private IUsersOperations usersOperations;

        private Uri CallbackUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url) { Path = Url.Action("Callback") };
                return uriBuilder.Uri;
            }
        }

        public AccountController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
            
        }

        public ActionResult Login()
        {
            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "162158623946329",
                client_secret = "a6b6d6e32f873889ef5a546ba68a4978",
                redirect_uri = this.CallbackUri.AbsoluteUri,
                response_type = "code",
                scope = "email" 
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult Callback(string code)
        {
            var fb = new FacebookClient();

            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "162158623946329",
                client_secret = "a6b6d6e32f873889ef5a546ba68a4978",
                redirect_uri = this.CallbackUri.AbsoluteUri,
                code = code
            });

            fb.AccessToken = result.access_token;

            dynamic me = fb.Get("me?fields=name,email,picture");

            if (this.usersOperations.Read(me.email) == null)
            {
                var user = new User(me.email)
                                {
                                    Friends = new List<User.UserSynopsis>(),
                                    Name = me.name,
                                    Photo = null
                                };

                this.usersOperations.Create(user);
            }

            FormsAuthentication.SetAuthCookie(me.email, false);

            return RedirectToAction("GetInfo", "Users");
        }
    }
}
