// --------------------------------------------------------------------------------------------------------------------
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
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Custom_action_results;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
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

            var model = new TvShowView(tvshow);
            return this.View(model);
        }

        /// <summary>
        /// The show options.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="unsubscribeRedirectUrl">
        /// The Unsubscribe Redirect Url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult TvShowPartialOptions(string tvshowId, string unsubscribeRedirectUrl)
        {
            var tvshow = this.tvshowOperations.Read(tvshowId);
            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }
            
            var isSubscribed = false;

            if (User.Identity.IsAuthenticated)
            {
                var user = this.userOperations.Read(User.Identity.Name);
                isSubscribed = user.SubscriptionList.Any(synopsis => synopsis.Id.Equals(tvshowId));
            }

            var tvshowPartial = new TvShowPartialOptions
                {
                    TvShowId = tvshowId, 
                    Poster = tvshow.Poster.ImageUrl, 
                    IsSubscribed = isSubscribed,
                    RedirectUrl = unsubscribeRedirectUrl
                };

            return this.View(tvshowPartial);
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
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            var view = new TvShowCommentsView { TvShowId = tvshowComments.TvShowId, Comments = tvshowComments.Comments };

            return this.View(view);
        }

        /// <summary>
        /// The add comment.
        /// </summary>
        /// <param name="addView">
        /// The operation View.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Comments(TvShowCommentAddView addView)
        {
            if (addView.Body != null && addView.Body.Trim().Equals(string.Empty))
            {
                ModelState.AddModelError("Body", "The comment is empty!");  
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Comments(addView.TvShowId);
            }

            var comment = new Comment { Body = addView.Body, UserId = User.Identity.Name, Timestamp = System.Environment.TickCount.ToString(CultureInfo.InvariantCulture) };

            this.commentsOperations.AddComment(addView.TvShowId, comment);
            return new SeeOtherResult { Url = Url.Action("Comments", "TvShowsWeb", new { addView.TvShowId }) };
        }

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

            var commentView = new TvShowCommentView { TvShowId = tvshowId, UserId = comment.UserId, Body = comment.Body, Timestamp = comment.Timestamp };
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
                return null;
            }

            this.commentsOperations.RemoveComment(removeView.TvShowId, User.Identity.Name, removeView.Timestamp);
            return new SeeOtherResult { Url = Url.Action("Comments", "TvShowsWeb", new { removeView.TvShowId }) };
        }
    }
}