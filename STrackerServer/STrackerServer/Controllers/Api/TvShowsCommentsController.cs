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

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
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
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsCommentsController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        /// <param name="usersOperations">
        /// The users Operations.
        /// </param>
        public TvShowsCommentsController(ITvShowsCommentsOperations operations, IUsersOperations usersOperations)
        {
            this.operations = operations;
            this.usersOperations = usersOperations;
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
        [HawkAuthorize]
        public HttpResponseMessage Post(string id, [FromBody] string comment)
        {
            if (comment == null || comment.Equals(string.Empty))
            {
                return this.BasePostDelete(false);
            }

            var user = this.usersOperations.Read(User.Identity.Name);

            return this.BasePostDelete(this.operations.AddComment(id, new Comment { Body = comment, User = user.GetSynopsis() }));
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="commentId">
        /// The comment Id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpDelete]
        [HawkAuthorize]
        public HttpResponseMessage Delete(string id, string commentId)
        {
            return this.BasePostDelete(this.operations.RemoveComment(id, User.Identity.Name, commentId));
        }
    }
}