// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The user web controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Action_Results;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Permissions;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Models.User;

    /// <summary>
    /// The user web controller.
    /// </summary>
    public class UserController : Controller
    {
        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// The permission manager.
        /// </summary>
        private readonly IPermissionManager<Permissions, int> permissionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        /// <param name="tvshowsOperations">
        /// The television shows operations. 
        /// </param>
        /// <param name="permissionManager">
        /// The permission manager.
        /// </param>
        public UserController(IUsersOperations usersOperations, ITvShowsOperations tvshowsOperations, IPermissionManager<Permissions, int> permissionManager)
        {
            this.usersOperations = usersOperations;
            this.tvshowsOperations = tvshowsOperations;
            this.permissionManager = permissionManager;
        }

        /// <summary>
        /// The authenticated user's profile.
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
                PictureUrl = user.Photo,
                SubscriptionList = user.Subscriptions,
                IsAdmin = this.permissionManager.HasPermission(Permissions.Admin, user.Permission),
                NewEpisodes = this.usersOperations.GetUserNewEpisodes(User.Identity.Name, DateTime.Now.AddDays(7)).OrderBy(calendar => calendar.Date).ToList()
            });
        }

        /// <summary>
        /// Invite a user to become his friend.
        /// </summary>
        /// <param name="values">
        /// The form values.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Invite(InviteFormValues values)
        {
            if (!ModelState.IsValid || !this.usersOperations.InviteFriend(User.Identity.Name, values.UserId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("BadRequest");
            }

            return new SeeOtherResult { Url = Url.Action("Index", "Users", new { id = values.UserId }) };
        }

        /// <summary>
        /// The authenticated user's friend requests.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult FriendRequests()
        {
            var user = this.usersOperations.Read(User.Identity.Name);

            return this.View(new Requests
            {
                Name = user.Name,
                PictureUrl = user.Photo,
                List = user.FriendRequests
            });
        }

        /// <summary>
        /// The authenticated user's friend requests count.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public JsonResult Updater()
        {
            var user = this.usersOperations.Read(User.Identity.Name);
            return this.Json(new { FriendRequests = user.FriendRequests.Count, Suggestions = user.Suggestions.Count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// The authenticated user's friend request response.
        /// </summary>
        /// <param name="values">
        /// The form values.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult FriendRequests(RequestResponseFormValues values)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("BadRequest");
            }

            if (values.Accept)
            {
                if (!this.usersOperations.AcceptInvite(values.UserId, User.Identity.Name))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return this.View("BadRequest");
                }
            }
            else
            {
                if (!this.usersOperations.RejectInvite(values.UserId, User.Identity.Name))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return this.View("BadRequest");
                }
            }

            return new SeeOtherResult { Url = Url.Action("FriendRequests") };
        }

        /// <summary>
        /// The authenticated user's friends.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Friends()
        {
            var user = this.usersOperations.Read(User.Identity.Name);
            return this.View(new FriendsView { Name = user.Name, List = user.Friends, PictureUrl = user.Photo });
        }

        /// <summary>
        /// Remove specific friend.
        /// </summary>
        /// <param name="id">
        /// The friend id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Friends(string id)
        {
            if (!this.usersOperations.RemoveFriend(User.Identity.Name, id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("BadRequest");
            }

            return new SeeOtherResult { Url = Url.Action("Friends") };
        }

        /// <summary>
        /// The authenticated user's suggestions.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Suggestions()
        {
            var user = this.usersOperations.Read(User.Identity.Name);
            var suggestionsView = new SuggestionsView
            {
                Name = user.Name,
                PictureUrl = user.Photo,
            };

            foreach (var suggestion in user.Suggestions.OrderBy(su => su.TvShow.Id))
            {
                if (!suggestionsView.Suggestions.ContainsKey(suggestion.TvShow.Id))
                {
                    suggestionsView.Suggestions.Add(suggestion.TvShow.Id, new SuggestionView { TvShowName = this.tvshowsOperations.Read(suggestion.TvShow.Id).Name });
                }

                suggestionsView.Suggestions[suggestion.TvShow.Id].Friends.Add(this.usersOperations.Read(suggestion.User.Id).GetSynopsis());
            }

            return this.View(suggestionsView);
        }

        /// <summary>
        /// Remove television show suggestions.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Suggestions(string tvshowId)
        {
            if (!this.usersOperations.RemoveTvShowSuggestions(User.Identity.Name, tvshowId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("BadRequest");
            }

            return new SeeOtherResult { Url = Url.Action("Suggestions") };
        }

        /// <summary>
        /// The authenticated user's watched episodes.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult WatchedEpisodes()
        {
            var user = this.usersOperations.Read(User.Identity.Name);

            var viewModel = new EpisodesWatchedView { Name = user.Name, PictureUrl = user.Photo };

            foreach (var subscription in user.Subscriptions)
            {
                var subDetailView = new EpisodesWatchedView.SubscriptionDetailView { TvShow = subscription.TvShow };

                foreach (var episode in subscription.EpisodesWatched)
                {
                    IList<Episode.EpisodeSynopsis> list;

                    if (subDetailView.EpisodesWatched.ContainsKey(episode.Id.SeasonNumber))
                    {
                        list = subDetailView.EpisodesWatched[episode.Id.SeasonNumber];
                    }
                    else
                    {
                        list = new List<Episode.EpisodeSynopsis>();
                        subDetailView.EpisodesWatched.Add(episode.Id.SeasonNumber, list);
                    }

                    list.Add(episode);
                }

                viewModel.List.Add(subDetailView);
            }

            return this.View(viewModel);
        }

        /// <summary>
        /// The authenticated user's calendar (iCal).
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Calendar()
        {
            return this.File(this.usersOperations.GetCalendar(User.Identity.Name), "text/calendar", "calendar.ics");
        }
    }
}