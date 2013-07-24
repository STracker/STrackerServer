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
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Permissions;
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
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// The permission manager.
        /// </summary>
        private readonly IPermissionManager<Permission, int> permissionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersWebController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        /// <param name="tvshowsOperations">
        /// The television shows operations. 
        /// </param>
        /// <param name="permissionManager">
        /// The permission Manager.
        /// </param>
        public UsersWebController(IUsersOperations usersOperations, ITvShowsOperations tvshowsOperations, IPermissionManager<Permission, int> permissionManager)
        {
            this.usersOperations = usersOperations;
            this.tvshowsOperations = tvshowsOperations;
            this.permissionManager = permissionManager;
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

            var currentUser = this.usersOperations.Read(User.Identity.Name);
            var isFriend = user.Friends.Any(synopsis => synopsis.Id.Equals(this.User.Identity.Name)) || user.FriendRequests.Any(synopsis => synopsis.Id.Equals(this.User.Identity.Name));
            var adminMode = this.permissionManager.HasPermission(Permission.Admin, currentUser.Permission);

            return this.View(new UserPublicView
            {
                Id = id,
                Name = user.Name,
                PictureUrl = user.Photo,
                SubscriptionList = user.SubscriptionList,
                IsFriend = isFriend,
                IsAdmin = this.permissionManager.HasPermission(Permission.Admin, user.Permission),
                AdminMode = adminMode
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
                PictureUrl = user.Photo,
                SubscriptionList = user.SubscriptionList,
                IsAdmin = this.permissionManager.HasPermission(Permission.Admin, user.Permission),
                NewEpisodes = this.usersOperations.GetNewestEpisodes(User.Identity.Name)
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

            return this.View(new Requests
                {
                    Name = user.Name,
                    PictureUrl = user.Photo,
                    List = user.FriendRequests       
                });
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
                return this.View(new UserSearchResult { Result = new List<User.UserSynopsis>(), SearchValue = string.Empty });
            }

            var users = this.usersOperations.FindByName(name);

            if (users.Count != 0)
            {
                var index = users.FindIndex(user => user.Id.Equals(User.Identity.Name));
                if (index != -1)
                {
                    users.RemoveAt(index); 
                }   
            }

            return this.View(new UserSearchResult
                {
                    Result = users,
                    SearchValue = name
                });
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
            return this.View(new FriendsView { Name = user.Name, List = user.Friends, PictureUrl = user.Photo });
        }

        /// <summary>
        /// The remove friend.
        /// </summary>
        /// <param name="friendId">
        /// The friend id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult RemoveFriend(string friendId)
        {
            if (!this.usersOperations.RemoveFriend(User.Identity.Name, friendId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Friends") };
        }

        /// <summary>
        /// The suggestions.
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

            foreach (var suggestion in user.Suggestions.OrderBy(su => su.TvShowId))
            {
                if (!suggestionsView.Suggestions.ContainsKey(suggestion.TvShowId))
                {
                    suggestionsView.Suggestions.Add(suggestion.TvShowId, new SuggestionView { TvShowName = this.tvshowsOperations.Read(suggestion.TvShowId).Name });
                }

                suggestionsView.Suggestions[suggestion.TvShowId].Friends.Add(this.usersOperations.Read(suggestion.UserId).GetSynopsis());
            }

            return this.View(suggestionsView);
        }

        /// <summary>
        /// The remove television show suggestions.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult RemoveTvShowSuggestions(string tvshowId)
        {
            if (!this.usersOperations.RemoveTvShowSuggestions(User.Identity.Name, tvshowId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Suggestions") };
        }

        /// <summary>
        /// The public friends.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult PublicFriends(string id)
        {
            if (User.Identity.Name.Equals(id))
            {
                return new SeeOtherResult { Url = Url.Action("Friends") };
            }

            var user = this.usersOperations.Read(id);

            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            return this.View(new PublicFriendsView
                {
                    Id = id,
                    Name = user.Name, 
                    List = user.Friends, 
                    PictureUrl = user.Photo
                });
        }

        /// <summary>
        /// The episodes watched.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult EpisodesWatched()
        {
            var user = this.usersOperations.Read(User.Identity.Name);

            var viewModel = new EpisodesWatchedView { Name = user.Name, PictureUrl = user.Photo };

            foreach (var subscription in user.SubscriptionList)
            {
                var subDetailView = new EpisodesWatchedView.SubscriptionDetailView { TvShow = subscription.TvShow };

                foreach (var episode in subscription.EpisodesWatched)
                {
                    IList<Episode.EpisodeSynopsis> list;

                    if (subDetailView.EpisodesWatched.ContainsKey(episode.SeasonNumber))
                    {
                        list = subDetailView.EpisodesWatched[episode.SeasonNumber];
                    }
                    else
                    {
                        list = new List<Episode.EpisodeSynopsis>();
                        subDetailView.EpisodesWatched.Add(episode.SeasonNumber, list);
                    }

                    list.Add(episode);
                }

                viewModel.List.Add(subDetailView);
            }

            return this.View(viewModel);
        }
    }
}
