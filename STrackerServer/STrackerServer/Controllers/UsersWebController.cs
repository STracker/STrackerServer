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

    using STrackerServer.Action_Results;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.DataAccessLayer.DomainEntities;
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
        [Authorize]
        public ActionResult Show(string id)
        {
            if (User.Identity.Name.Equals(id))
            {
                return new SeeOtherResult { Url = Url.Action("Index") };
            }

            var user = this.usersOperations.Read(id);
            
            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var isFriend = user.Friends.Any(synopsis => synopsis.Id.Equals(this.User.Identity.Name)) || user.FriendRequests.Any(synopsis => synopsis.Id.Equals(this.User.Identity.Name));

            return this.View(new UserPublicView
            {
                Id = id,
                Name = user.Name,
                PictureUrl = user.Photo.ImageUrl,
                SubscriptionList = user.SubscriptionList,
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

            var requests = new Requests
                {
                    Name = user.Name,
                    PictureUrl = user.Photo.ImageUrl,
                    List =
                        user.FriendRequests.ConvertAll(
                            input =>
                            new Requests.FriendRequest
                                {
                                    Id = input.Id,
                                    Name = input.Name,
                                    Picture = this.usersOperations.Read(input.Id).Photo.ImageUrl
                                })
                };

            return this.View(requests);
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

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult GetByName(string name)
        {
            if (name == null || string.Empty.Equals(name.Trim()))
            {
                return this.View(new UserSearchResult { Result = new List<User>(), SearchValue = string.Empty });
            }

            var users = this.usersOperations.FindByName(name);

            if (users.Count != 0)
            {
                var index = users.FindIndex(user => user.Key.Equals(User.Identity.Name));
                if (index != -1)
                {
                    users.RemoveAt(index); 
                }   
            }

            var result = new UserSearchResult
            {
                Result = users,
                SearchValue = name
            };
            return this.View(result);
        }

        /// <summary>
        /// The user friends.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Friends()
        {
            var user = this.usersOperations.Read(User.Identity.Name);
            var view = new FriendsView { Name = user.Name, List = user.Friends, PictureUrl = user.Photo.ImageUrl };
            return this.View(view);
        }

        public ActionResult RemoveFriend(string friendId)
        {
            throw new System.NotImplementedException();
        }
    }
}
