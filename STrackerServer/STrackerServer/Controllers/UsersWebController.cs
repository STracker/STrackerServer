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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using STrackerServer.BusinessLayer.Core;
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
        /// The friend request operations.
        /// </summary>
        private readonly IFriendRequestOperations friendRequestOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersWebController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        /// <param name="friendRequestOperations">
        /// The friend Request Operations.
        /// </param>
        public UsersWebController(IUsersOperations usersOperations, IFriendRequestOperations friendRequestOperations)
        {
            this.usersOperations = usersOperations;
            this.friendRequestOperations = friendRequestOperations;
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
                if (user.Friends.Any(synopsis => synopsis.Id.Equals(User.Identity.Name))
                    || this.friendRequestOperations.Read(User.Identity.Name, id) != null)
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

            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

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
            List<FriendRequest> requests = this.friendRequestOperations.GetRequests(User.Identity.Name);

            if (requests == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return this.View(requests.ConvertAll(request =>
                {
                    User user = usersOperations.Read(request.From);
                    return new FriendRequestView { Id = request.Key, Name = user.Name, Picture = user.Photo.ImageUrl };
                }));
        }

        /// <summary>
        /// The invite.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Invite(string id)
        {
            if (id.Equals(User.Identity.Name))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            FriendRequest request = new FriendRequest
                {
                    From = User.Identity.Name, 
                    To = id,
                    Accepted = false
                };

            if (!this.friendRequestOperations.Create(request))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Show", new { id }) };
        }

        /// <summary>
        /// The subscribe.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Subscribe(string tvshowId)
        {
            if (!this.usersOperations.AddSubscription(User.Identity.Name, tvshowId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Show", "TvShowsWeb", new { tvshowId }) };
        }

        /// <summary>
        /// The un subscribe.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="redirectUrl">
        /// The redirect url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult UnSubscribe(string tvshowId, string redirectUrl)
        {
            if (!this.usersOperations.RemoveSubscription(User.Identity.Name, tvshowId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = redirectUrl };
        }

        /// <summary>
        /// The accept request.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="accept">
        /// The accept.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult AcceptRequest(string id, bool accept)
        {
            if (accept)
            {
                if (!this.friendRequestOperations.Accept(id, User.Identity.Name))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return this.View("Error", Response.StatusCode);
                }
            }
            else
            {
                if (!this.friendRequestOperations.Reject(id, User.Identity.Name))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return this.View("Error", Response.StatusCode);
                }
            }

            return new SeeOtherResult { Url = Url.Action("Requests") };
        }
    }
}
