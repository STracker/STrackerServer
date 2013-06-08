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
    using System.Globalization;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Custom_action_results;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.Models.Partial;
    using STrackerServer.Models.TvShow;

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

            var model = new TvShowView(tvshow)
                {
                    Options =
                        TvShowOptionsView.Create(
                            tvshow, this.userOperations.Read(User.Identity.Name), Url.Action("Show", new { tvshowId }))
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
            var tvshow = this.tvshowOperations.ReadByName(name);
            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Show", new { tvshowId = tvshow.TvShowId }) };
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

            var view = new TvShowCommentsView
                {
                    TvShowId = tvshowComments.TvShowId,
                    Comments = tvshowComments.Comments,
                    Options = new TvShowCommentsOptionsView
                        {
                            Poster = tvshow.Poster.ImageUrl,
                            TvShowId = tvshowId,
                            TvShowName = tvshow.Name
                        }
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

            var view = new TvShowCreateComment
                {
                    TvShowId = tvshowId,
                    Options = new TvShowCreateCommentOptions { Poster = tvshow.Poster.ImageUrl, TvShowId = tvshowId }
                };

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
            if (create.Body != null && create.Body.Trim().Equals(string.Empty))
            {
                ModelState.AddModelError("Body", "The comment is empty!");  
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                this.View(create);
            }

            var comment = new Comment { Body = create.Body, UserId = User.Identity.Name, Timestamp = System.Environment.TickCount.ToString(CultureInfo.InvariantCulture) };

            this.commentsOperations.AddComment(create.TvShowId, comment);
            return new SeeOtherResult { Url = Url.Action("Comments", "TvShowsWeb", new { create.TvShowId }) };
        }

        /*
        /// <summary>
        /// The comments remove.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show Key.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult CommentsEdit(string tvshowId, int position)
        {
            var comments = this.commentsOperations.GetComments(tvshowId).Comments;

            if (position >= comments.Count)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var comment = comments.ElementAt(position);

            if (!comment.UserId.Equals(User.Identity.Name))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.View("Error", Response.StatusCode); 
            }

            var tvshow = this.tvshowOperations.Read(tvshowId);

            var commentView = new TvShowCommentView
                {
                    TvShowId = tvshowId, 
                    UserId = comment.UserId, 
                    Body = comment.Body, 
                    Timestamp = comment.Timestamp,
                    Options = TvShowOptionsView.Create(tvshow, this.userOperations.Read(User.Identity.Name), Url.Action("CommentsEdit", new { tvshowId, position }))
                };
            return this.View(commentView);
        }

        /// <summary>
        /// The comments remove.
        /// </summary>
        /// <param name="removeView">
        /// The remove View.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult CommentsEdit(TvShowCommentRemoveView removeView)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            this.commentsOperations.RemoveComment(removeView.TvShowId, User.Identity.Name, removeView.Timestamp);
            return new SeeOtherResult { Url = Url.Action("Comments", "TvShowsWeb", new { removeView.TvShowId }) };
        }
         * */
    }
}