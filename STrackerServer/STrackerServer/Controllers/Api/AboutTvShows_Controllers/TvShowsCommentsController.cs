// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsCommentsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for television shows comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutTvShows_Controllers
{
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
        /// Operations object.
        /// </summary>
        private readonly ITvShowsCommentsOperations operations;

        /// <summary>
        /// Users operations object.
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
        /// Get all comments from one television show.
        /// </summary>
        /// <param name="id">
        /// The id of the television show.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            var comments = this.operations.Read(id);
            return this.BaseGet(comments);
        }

        /// <summary>
        /// Create one comment for the television show.
        /// </summary>
        /// <param name="id">
        /// The id of the television show.
        /// </param>
        /// <param name="comment">
        /// The user's comment.
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

            var user = this.usersOperations.Read(this.User.Identity.Name);
            return this.BasePostDelete(this.operations.AddComment(id, new Comment { Body = comment, User = user.GetSynopsis() }));
        }

        /// <summary>
        /// Deletes one comment from television show.
        /// </summary>
        /// <param name="id">
        /// The id of the television show.
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
            return this.BasePostDelete(this.operations.RemoveComment(id, this.User.Identity.Name, commentId));
        }
    }
}