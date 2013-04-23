using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Facebook;
using STrackerServer.BusinessLayer.Core;
using STrackerServer.DataAccessLayer.DomainEntities;
using System.Configuration;

namespace STrackerServer.Controllers
{
    public class AccountController : Controller
    {
        private IUsersOperations usersOperations;

        private Uri CallbackUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url)
                                     {
                                         Path = Url.Action("Callback"),
                                     };
                return uriBuilder.Uri;
            }
        }

        private static readonly string FacebookClientId = ConfigurationManager.AppSettings["Client:Id"];
        private static readonly string FacebookClientSecret = ConfigurationManager.AppSettings["Client:Secret"];

        public AccountController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
            
        }

        public ActionResult Login()
        {
            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = FacebookClientId,
                redirect_uri = this.CallbackUri.AbsoluteUri,
                response_type = "code",
                scope = "email,publish_actions,read_stream" 
            });

            return this.Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult Callback(string code)
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

            fb.AccessToken = result.access_token;

            dynamic me = fb.Get("me?fields=name,email,picture");

            var user = this.usersOperations.Read((string) me.email);

            var newUser = new User(me.email)
                              {
                                  Friends = new List<User.UserSynopsis>(),
                                  Name = me.name,
                                  Photo = new Artwork
                                              {
                                                  ImageUrl = me.picture.data.url
                                              }
                              };


            if (user == null)
            {
                this.usersOperations.Create(newUser);
            }
            else
            {
                if (user.CompareTo(newUser) == 0)
                {
                    newUser.Friends = user.Friends;
                    this.usersOperations.Update(newUser);
                }
            }


            FormsAuthentication.SetAuthCookie(me.email, false);

            return this.RedirectToAction("GetInfo", "Users");
        }
    }
}
