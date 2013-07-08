﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsWebController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Account Controller.
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
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.Models.TvShow;
    using STrackerServer.Models.User;

    /// <summary>
    /// The television shows web controller.
    /// </summary>
    public class TvShowsWebController : Controller
    {
        /// <summary>
        /// The operations of the Television Show Controller.
        /// </summary>
        private readonly ITvShowsOperations tvshowOperations;

        /// <summary>
        /// The user operations.
        /// </summary>
        private readonly IUsersOperations userOperations;

        /// <summary>
        /// The comments operations.
        /// </summary>
        private readonly ITvShowsCommentsOperations commentsOperations;

        /// <summary>
        /// The ratings operations.
        /// </summary>
        private readonly ITvShowsRatingsOperations ratingsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsWebController"/> class.
        /// </summary>
        /// <param name="tvshowOperations">
        /// The television show operations.
        /// </param>
        /// <param name="userOperations">
        /// The user operations.
        /// </param>
        /// <param name="commentsOperations">
        /// The comments Operations.
        /// </param>
        /// <param name="ratingsOperations">
        /// The ratings Operations.
        /// </param>
        public TvShowsWebController(ITvShowsOperations tvshowOperations, IUsersOperations userOperations, ITvShowsCommentsOperations commentsOperations, ITvShowsRatingsOperations ratingsOperations)
        {
            this.tvshowOperations = tvshowOperations;
            this.userOperations = userOperations;
            this.commentsOperations = commentsOperations;
            this.ratingsOperations = ratingsOperations;
        }

        /// <summary>
        /// The television show
        /// </summary>
        /// <param name="tvshowId">
        /// The television Show Key.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Show(string tvshowId)
        {
            var tvshow = this.tvshowOperations.Read(tvshowId);

            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var isSubscribed = false;

            var user = this.userOperations.Read(User.Identity.Name);

            if (user != null)
            {
                isSubscribed = user.SubscriptionList.Any(synopsis => synopsis.Id.Equals(tvshow.TvShowId));
            }

            var model = new TvShowView(tvshow)
            {
                Rating = this.ratingsOperations.Read(tvshowId).Average,
                IsSubscribed = isSubscribed,
                Poster = tvshow.Poster,
                RedirectUrl = Url.Action("Show", new { tvshowId })
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
            if (name == null || string.Empty.Equals(name.Trim()))
            {
                return this.View(new TvShowSearchResult { Result = new List<TvShow.TvShowSynopsis>(), SearchValue = string.Empty });
            }

            var tvshows = this.tvshowOperations.ReadByName(name);
            var result = new TvShowSearchResult
                {
                    Result = tvshows,
                    SearchValue = name
                };

            return this.View(result);
        }

        /// <summary>
        /// The comments.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Comments(string tvshowId)
        {
            var tvshowComments = this.commentsOperations.GetComments(tvshowId);

            if (tvshowComments == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var tvshow = this.tvshowOperations.Read(tvshowId);

            var view = new TvShowComments
                {
                    TvShowId = tvshowComments.TvShowId,
                    Comments = tvshowComments.Comments,
                    Poster = tvshow.Poster,
                    TvShowName = tvshow.Name
                };
            return this.View(view);
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

            var view = new TvShowCreateComment { TvShowId = tvshowId, Poster = tvshow.Poster };

            return this.View(view);
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

            var comment = new Comment { Body = create.Body, UserId = User.Identity.Name };

            this.commentsOperations.AddComment(create.TvShowId, comment);
            return new SeeOtherResult { Url = Url.Action("Comments", "TvShowsWeb", new { create.TvShowId }) };
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
        [Authorize]
        public ActionResult Comment(string tvshowId, string id)
        {
            var comments = this.commentsOperations.GetComments(tvshowId).Comments;

            var comment = comments.Find(comment1 => comment1.Id.Equals(id));

            if (comment == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode); 
            }

            if (!comment.UserId.Equals(User.Identity.Name))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.View("Error", Response.StatusCode); 
            }

            var tvshow = this.tvshowOperations.Read(tvshowId);

            var commentView = new TvShowComment
                {
                    TvShowId = tvshowId, 
                    UserId = comment.UserId, 
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
        [Authorize]
        public ActionResult RemoveComment(TvShowRemoveComment removeView)
        {
            if (!ModelState.IsValid || !this.commentsOperations.RemoveComment(removeView.TvShowId, User.Identity.Name, removeView.Id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Comments", "TvShowsWeb", new { removeView.TvShowId }) };
        }

        /// <summary>
        /// The suggestion.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Suggest(string tvshowId)
        {
            var tvshow = this.tvshowOperations.Read(tvshowId);

            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var user = this.userOperations.Read(User.Identity.Name);


            var view = new SuggestView
                {
                    Friends = user.Friends.ConvertAll(input => new SuggestFriendView
                        {
                            Id = input.Id,
                            Name = input.Name,
                            IsSubscribed = this.userOperations.Read(input.Id).SubscriptionList.Exists(synopsis => synopsis.Id.Equals(tvshowId))
                        }), 
                    TvShowId = tvshowId,
                    Poster = tvshow.Poster,
                    TvShowName = tvshow.Name
                };

            return this.View(view);
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
            if (!ModelState.IsValid || !this.userOperations.SendSuggestion(values.FriendId, values.TvShowId, new Suggestion { TvShowId = values.TvShowId, UserId = User.Identity.Name }))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Suggest", new { tvshowId = values.TvShowId }) };
        }

        /// <summary>
        /// The rating.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Rate(string tvshowId)
        {
            var tvshow = this.tvshowOperations.Read(tvshowId);

            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            return this.View(new TvShowRating
                        {
                            TvShowId = tvshowId,
                            TvShowName = tvshow.Name,
                            Poster = tvshow.Poster,
                            Value = 1
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
            var tvshow = this.tvshowOperations.Read(rating.TvShowId); 

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

            if (tvshow == null || !this.ratingsOperations.AddRating(tvshow.TvShowId, new Rating { UserId = User.Identity.Name, UserRating = rating.Value }))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Show", new { tvshowId = rating.TvShowId }) };
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
            return this.Json(query == null ? new string[0] : this.tvshowOperations.GetNames(query), JsonRequestBehavior.AllowGet);
        }
    }
}