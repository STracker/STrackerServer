// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersWebController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Account Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Custom_action_results;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Models.TvShow;
    using STrackerServer.Models.User;

    /// <summary>
    /// The users controller.
    /// </summary>
    [Authorize]
    public class UsersWebController : Controller
    {
        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersWebController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        public UsersWebController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Show(string id)
        {
            if (User.Identity.IsAuthenticated && User.Identity.Name.Equals(id))
            {
                return new SeeOtherResult { Url = Url.Action("Index") };
            }

            var user = this.usersOperations.Read(id);
            
            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var isFriend = false;
            
            if (this.User.Identity.IsAuthenticated)
            {
                if (user.Friends.Any(synopsis => synopsis.Id.Equals(User.Identity.Name)) || user.FriendRequests.Any(synopsis => synopsis.Id.Equals(User.Identity.Name)))
                {
                    isFriend = true;
                }
            }
            
            return this.View(new UserPublicView
            {
                Id = id,
                Name = user.Name,
                PictureUrl = user.Photo.ImageUrl,
                SubscriptionList = user.SubscriptionList.ConvertAll(input => new SubscriptionView
                    {
                        Id = input.Id,
                        Name = input.Name
                    }),
                IsFriend = isFriend
            });
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var user = this.usersOperations.Read(User.Identity.Name);

            return this.View(new UserPrivateView
            {
                Id = User.Identity.Name,
                Name = user.Name,
                PictureUrl = user.Photo.ImageUrl,
                SubscriptionList = user.SubscriptionList,
            });
        }

        /// <summary>
        /// The requests.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Requests()
        {
            var user = this.usersOperations.Read(User.Identity.Name);

            return this.View(user.FriendRequests.ConvertAll(input => 
                {
                    User userReq = usersOperations.Read(input.Id);
                    return new FriendRequestView { Id = userReq.Key, Name = userReq.Name, Picture = userReq.Photo.ImageUrl };
                }));
        }

        /// <summary>
        /// The invite.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Invite(InviteFormValues values)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            if (!this.usersOperations.Invite(User.Identity.Name, values.Id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Show", new { id = values.Id }) };
        }

        /// <summary>
        /// The subscribe.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Subscribe(SubscribeFormValues values)
        {
            if (!ModelState.IsValid || !this.usersOperations.AddSubscription(User.Identity.Name, values.TvshowId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = values.RedirectUrl };
        }

        /// <summary>
        /// The un subscribe.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult UnSubscribe(UnsubscribeFormValues values)
        {
            if (!ModelState.IsValid || !this.usersOperations.RemoveSubscription(User.Identity.Name, values.TvshowId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = values.RedirectUrl };
        }

        /// <summary>
        /// The request.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult RequestResponse(string id, RequestResponseFormValues values)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            if (values.Accept)
            {
                if (!this.usersOperations.AcceptInvite(id, User.Identity.Name))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return this.View("Error", Response.StatusCode);
                }
            }
            else
            {
                if (!this.usersOperations.RejectInvite(id, User.Identity.Name))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return this.View("Error", Response.StatusCode);
                }
            }

            return new SeeOtherResult { Url = Url.Action("Requests") };
        }
    }
}
