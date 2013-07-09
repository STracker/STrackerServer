﻿// --------------------------------------------------------------------------------------------------------------------
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
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.Hawk;

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
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesCommentsController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        /// <param name="usersOperations">
        /// The users Operations.
        /// </param>
        public EpisodesCommentsController(IEpisodesCommentsOperations operations, IUsersOperations usersOperations)
        {
            this.operations = operations;
            this.usersOperations = usersOperations;
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
        [HawkAuthorize]
        public HttpResponseMessage Post(string tvshowId, int seasonNumber, int number, [FromBody] [Required] string comment)
        {
            if (!ModelState.IsValid)
            {
                return this.BasePost(false);
            }

            var user = this.usersOperations.Read(User.Identity.Name);

            this.operations.AddComment(new Tuple<string, int, int>(tvshowId, seasonNumber, number), new Comment { Body = comment, User = user.GetSynopsis() });
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
        /// <param name="commentId">
        /// The comment id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpDelete]
        [HawkAuthorize]
        public HttpResponseMessage Delete(string tvshowId, int seasonNumber, int number, string userId, string commentId)
        {
            return this.BasePost(this.operations.RemoveComment(new Tuple<string, int, int>(tvshowId, seasonNumber, number), userId, commentId));
        }
    }
}