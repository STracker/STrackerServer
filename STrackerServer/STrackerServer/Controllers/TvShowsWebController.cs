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
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.Custom_action_results;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Models.TvShow;

    /// <summary>
    /// The television shows web controller.
    /// </summary>
    public class TvShowsWebController : Controller
    {
        /// <summary>
        /// The operations of the Television Show Controller.
        /// </summary>
        private readonly ITvShowsOperations tvshowOps;

        /// <summary>
        /// The user operations.
        /// </summary>
        private readonly IUsersOperations userOps;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsWebController"/> class.
        /// </summary>
        /// <param name="tvshowOps">
        /// The television show operations.
        /// </param>
        /// <param name="userOps">
        /// The user operations.
        /// </param>
        public TvShowsWebController(ITvShowsOperations tvshowOps, IUsersOperations userOps)
        {
            this.tvshowOps = tvshowOps;
            this.userOps = userOps;
        }

        /// <summary>
        /// The television show
        /// </summary>
        /// <param name="tvshowId">
        /// The television Show Id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Show(string tvshowId)
        {
            var tvshow = this.tvshowOps.Read(tvshowId);

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
            var tvshow = this.tvshowOps.Read(tvshowId);

            var isSubscribed = false;

            if (User.Identity.IsAuthenticated)
            {
                var user = this.userOps.Read(User.Identity.Name);
                isSubscribed = user.SubscriptionList.Any(synopsis => synopsis.Id.Equals(tvshowId));
            }

            var tvshowPartial = new TvShowPartialOptions
                {
                    TvShowId = tvshowId, 
                    Poster = tvshow.Artworks[0].ImageUrl, 
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
            var tvshow = this.tvshowOps.ReadByName(name);
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
            var tvshowComments = this.tvshowOps.GetComments(tvshowId);

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
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Comments(string tvshowId, string body)
        {
            var comment = new Comment { UserId = User.Identity.Name, Body = body };

            if (!this.tvshowOps.AddComment(tvshowId, comment))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Comments", "TvShowsWeb", new { tvshowId }) };
        }
    }
}