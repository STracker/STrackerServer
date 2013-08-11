// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Television Shows Web Controller
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Action_Results;
    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Permissions;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.Models.TvShow;
    using STrackerServer.Models.User;

    /// <summary>
    /// The television shows web controller.
    /// </summary>
    public class TvShowsController : Controller
    {
        /// <summary>
        /// The operations television show controller.
        /// </summary>
        private readonly ITvShowsOperations tvshowOperations;

        /// <summary>
        /// The user operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// The comments operations.
        /// </summary>
        private readonly ITvShowsCommentsOperations commentsOperations;

        /// <summary>
        /// The ratings operations.
        /// </summary>
        private readonly ITvShowsRatingsOperations ratingsOperations;

        /// <summary>
        /// The permission manager.
        /// </summary>
        private readonly IPermissionManager<Permissions, int> permissionManager;

        /// <summary>
        /// The new television show episodes operations.
        /// </summary>
        private readonly ITvShowNewEpisodesOperations tvshowNewEpisodesOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsController"/> class.
        /// </summary>
        /// <param name="tvshowOperations">
        /// The television show operations.
        /// </param>
        /// <param name="usersOperations">
        /// The user operations.
        /// </param>
        /// <param name="commentsOperations">
        /// The comments Operations.
        /// </param>
        /// <param name="ratingsOperations">
        /// The ratings Operations.
        /// </param>
        /// <param name="permissionManager">
        /// The permission Manager.
        /// </param>
        /// <param name="tvshowNewEpisodesOperations">
        /// The new television show episodes operations.
        /// </param>
        public TvShowsController(ITvShowsOperations tvshowOperations, IUsersOperations usersOperations, ITvShowsCommentsOperations commentsOperations, ITvShowsRatingsOperations ratingsOperations, IPermissionManager<Permissions, int> permissionManager, ITvShowNewEpisodesOperations tvshowNewEpisodesOperations)
        {
            this.tvshowOperations = tvshowOperations;
            this.usersOperations = usersOperations;
            this.commentsOperations = commentsOperations;
            this.ratingsOperations = ratingsOperations;
            this.permissionManager = permissionManager;
            this.tvshowNewEpisodesOperations = tvshowNewEpisodesOperations;
        }

        /// <summary>
        /// The television show view
        /// </summary>
        /// <param name="id">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Index(string id)
        {
            var tvshow = this.tvshowOperations.Read(id);

            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var isSubscribed = false;
            Rating userRating = null;
            var ratings = this.ratingsOperations.Read(id);

            if (User.Identity.IsAuthenticated)
            {
                var user = this.usersOperations.Read(User.Identity.Name);

                isSubscribed = user.Subscriptions.Any(sub => sub.TvShow.Id.Equals(tvshow.Id));
                userRating = ratings.Ratings.Find(rating => rating.User.Id.Equals(user.Id));
            }

            var model = new TvShowView(tvshow)
            {
                Rating = (int)ratings.Average,
                IsSubscribed = isSubscribed,
                Poster = tvshow.Poster,
                UserRating = userRating != null ? userRating.UserRating : -1,
                RatingsCount = ratings.Ratings.Count,
                NewEpisodes = this.tvshowNewEpisodesOperations.GetNewEpisodes(tvshow.Id, null)
            };

            return this.View(model);
        }

        /// <summary>
        /// Get one television show by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult GetByName(string name)
        {
            var nameNormalized = name != null ? name.Trim().ToLower() : null;

            var tvshows = this.tvshowOperations.ReadByName(nameNormalized);

            if (tvshows.Count == 0)
            {
                return this.View("NotFound");
            }

            foreach (var tvshow in tvshows)
            {
                if (tvshow.Name.ToLower().Equals(nameNormalized))
                {
                    return new SeeOtherResult { Url = Url.Action("Index", "TvShows", new { id = tvshow.Id }) };
                }
            }

            var result = new TvShowSearchResult
            {
                Result = tvshows,
                SearchValue = name
            };

            return this.View(result);
        }

        /// <summary>
        /// The comments of a television show.
        /// </summary>
        /// <param name="id">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Comments(string id)
        {
            var tvshowComments = this.commentsOperations.Read(id);

            if (tvshowComments == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var tvshow = this.tvshowOperations.Read(id);
            var isModerator = false;

            if (User.Identity.IsAuthenticated)
            {
                var user = this.usersOperations.Read(User.Identity.Name);
                isModerator = this.permissionManager.HasPermission(Permissions.Moderator, user.Permission);
            }

            return this.View(new TvShowComments
            {
                TvShowId = tvshowComments.Id,
                Comments = tvshowComments.Comments,
                Poster = tvshow.Poster,
                TvShowName = tvshow.Name,
                IsModerator = isModerator
            });
        }

        /// <summary>
        /// The create comment.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult CreateComment(string tvshowId)
        {
            var tvshow = this.tvshowOperations.Read(tvshowId);

            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            return this.View(new TvShowCreateComment { TvShowId = tvshowId, Poster = tvshow.Poster });
        }

        /// <summary>
        /// The create comment.
        /// </summary>
        /// <param name="create">
        /// The create.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult CreateComment(TvShowCreateComment create)
        {
            var tvshow = this.tvshowOperations.Read(create.TvShowId);

            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                create.Poster = tvshow.Poster;
                return this.View(create);
            }

            this.commentsOperations.AddComment(
                create.TvShowId,
                new Comment
                {
                    Body = create.Body,
                    User = this.usersOperations.Read(User.Identity.Name).GetSynopsis()
                });

            return new SeeOtherResult { Url = Url.Action("Comments", "TvShows", new { id = create.TvShowId }) };
        }

        /// <summary>
        /// The comments edit.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [TvShowCommentAuthorize(Permissions = Permissions.Moderator, Owner = true)]
        public ActionResult Comment(string tvshowId, string id)
        {
            var user = this.usersOperations.Read(User.Identity.Name);

            var comments = this.commentsOperations.Read(tvshowId).Comments;
            var comment = comments.Find(com => com.Id.Equals(id));

            if (comment == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            if (!comment.User.Id.Equals(User.Identity.Name) && !this.permissionManager.HasPermission(Permissions.Moderator, user.Permission))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.View("Error", Response.StatusCode);
            }

            var tvshow = this.tvshowOperations.Read(tvshowId);

            var commentView = new TvShowComment
            {
                TvShowId = tvshowId,
                UserId = comment.User.Id,
                Body = comment.Body,
                Id = comment.Id,
                Poster = tvshow.Poster
            };

            return this.View(commentView);
        }

        /// <summary>
        /// The remove comment.
        /// </summary>
        /// <param name="removeView">
        /// The remove view.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [TvShowCommentAuthorize(Permissions = Permissions.Moderator, Owner = true)]
        public ActionResult RemoveComment(TvShowRemoveComment removeView)
        {
            if (!ModelState.IsValid || !this.commentsOperations.RemoveComment(removeView.TvShowId, User.Identity.Name, removeView.Id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Comments", "TvShows", new { id = removeView.TvShowId }) };
        }

        /// <summary>
        /// The suggestion.
        /// </summary>
        /// <param name="id">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Suggest(string id)
        {
            var tvshow = this.tvshowOperations.Read(id);

            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var user = this.usersOperations.Read(User.Identity.Name);

            return this.View(new SuggestView
            {
                Friends = user.Friends.ConvertAll(input => new SuggestFriendView
                {
                    Id = input.Id,
                    Name = input.Name,
                    IsSubscribed = this.usersOperations.Read(input.Id).Subscriptions.Exists(sub => sub.TvShow.Id.Equals(id))
                }),
                TvShowId = id,
                Poster = tvshow.Poster,
                TvShowName = tvshow.Name
            });
        }

        /// <summary>
        /// The suggestion.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Suggest(SuggestFormValues values)
        {
            if (!ModelState.IsValid || !this.usersOperations.SendSuggestion(User.Identity.Name, values.FriendId, values.Id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Suggest", new { id = values.Id }) };
        }

        /// <summary>
        /// The rating.
        /// </summary>
        /// <param name="id">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Rate(string id)
        {
            var tvshow = this.tvshowOperations.Read(id);

            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var user = this.usersOperations.Read(User.Identity.Name);
            var ratings = this.ratingsOperations.Read(id);
            var userRating = ratings.Ratings.Find(rating => rating.User.Id.Equals(user.Id));
            var userRatingValue = -1;

            if (userRating != null)
            {
                userRatingValue = userRating.UserRating;
            }

            return this.View(new TvShowRating
            {
                Id = id,
                TvShowName = tvshow.Name,
                Poster = tvshow.Poster,
                Value = userRatingValue != -1 ? userRatingValue : 1,
                Rating = (int)ratings.Average,
                RatingsCount = ratings.Ratings.Count
            });
        }

        /// <summary>
        /// The rating.
        /// </summary>
        /// <param name="rating">
        /// The form values.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Rate(TvShowRating rating)
        {
            var tvshow = this.tvshowOperations.Read(rating.Id);

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                if (tvshow == null)
                {
                    return this.View("Error", Response.StatusCode);
                }

                rating.TvShowName = tvshow.Name;
                rating.Poster = tvshow.Poster;

                return this.View(rating);
            }

            if (tvshow == null || !this.ratingsOperations.AddRating(tvshow.Id, new Rating { User = this.usersOperations.Read(User.Identity.Name).GetSynopsis(), UserRating = rating.Value }))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Index", new { id = rating.Id }) };
        }

        /// <summary>
        /// The get names.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult GetNames(string query)
        {
            return this.Json(query == null ? new string[0] : this.tvshowOperations.DirectReadByName(query).Select(synopsis => synopsis.Name), JsonRequestBehavior.AllowGet);
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
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            bool success;

            if (values.IsSubscribing)
            {
                success = this.usersOperations.AddSubscription(User.Identity.Name, values.Id);

                if (success)
                {
                    this.usersOperations.RemoveTvShowSuggestions(User.Identity.Name, values.Id);
                }
            }
            else
            {
                success = this.usersOperations.RemoveSubscription(User.Identity.Name, values.Id);
            }

            if (!success)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = values.RedirectUrl };
        }
    }
}
