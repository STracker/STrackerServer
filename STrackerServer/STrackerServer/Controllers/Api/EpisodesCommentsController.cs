// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesCommentsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for episodes comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.Controllers.Api.AuxiliaryObjects;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The episodes comments controller.
    /// </summary>
    public class EpisodesCommentsController : BaseController
    {
        /// <summary>
        /// The operations.
        /// </summary>
        private readonly IEpisodesCommentsOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesCommentsController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        public EpisodesCommentsController(IEpisodesCommentsOperations operations)
        {
            this.operations = operations;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get(string tvshowId, int seasonNumber, int number)
        {
            var comments = this.operations.GetComments(new Tuple<string, int, int>(tvshowId, seasonNumber, number));
            if (comments == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return this.BaseGet(comments.Comments);
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Post(string tvshowId, int seasonNumber, int number, [FromBody] ApiAddComment comment)
        {
            if (!ModelState.IsValid)
            {
                return this.BasePost(false);
            }

            this.operations.AddComment(new Tuple<string, int, int>(tvshowId, seasonNumber, number), new Comment { Body = comment.Body, UserId = comment.UserId });
            return this.BasePost(true);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="number">
        /// The number.
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
        public HttpResponseMessage Delete(string tvshowId, int seasonNumber, int number, string userId, string timestamp)
        {
            return this.BasePost(this.operations.RemoveComment(new Tuple<string, int, int>(tvshowId, seasonNumber, number), userId, timestamp));
        }
    }
}