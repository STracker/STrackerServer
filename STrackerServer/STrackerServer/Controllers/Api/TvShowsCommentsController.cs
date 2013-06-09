// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsCommentsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for television shows comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.Controllers.Api.AuxiliaryObjects;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The television shows comments controller.
    /// </summary>
    public class TvShowsCommentsController : BaseController
    {
        /// <summary>
        /// The operations.
        /// </summary>
        private readonly ITvShowsCommentsOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsCommentsController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        public TvShowsCommentsController(ITvShowsCommentsOperations operations)
        {
            this.operations = operations;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            var comments = this.operations.GetComments(id);
            if (comments == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return this.BaseGet(comments.Comments);
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Post(string id, [FromBody] ApiAddComment comment)
        {
            if (!ModelState.IsValid)
            {
                return this.BasePostOrDelete(false);
            }

            this.operations.AddComment(id, new Comment { Body = comment.Body, UserId = comment.UserId, Timestamp = comment.Timestamp });
            return this.BasePostOrDelete(true);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="timestamp">
        /// The timestamp.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpDelete]
        public HttpResponseMessage Delete(string id, string userId, string timestamp)
        {
            return this.BasePostOrDelete(this.operations.RemoveComment(id, userId, timestamp));
        }
    }
}