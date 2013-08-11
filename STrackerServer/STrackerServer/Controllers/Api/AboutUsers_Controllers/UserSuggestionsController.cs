// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserSuggestionsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api controller for user's suggestions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutUsers_Controllers
{
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Controllers.Api.AuxiliaryObjects;

    /// <summary>
    /// The user suggestions controller.
    /// </summary>
    public class UserSuggestionsController : BaseController
    {
        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSuggestionsController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        public UserSuggestionsController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// Get all user's suggestions.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [HawkAuthorize]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.usersOperations.Read(this.User.Identity.Name).Suggestions);
        }

        /// <summary>
        /// Send an suggestion to one user.
        /// </summary>
        /// <param name="request">
        /// The request object that encapsulates the information for send an suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        [HawkAuthorize]
        public HttpResponseMessage Post([FromBody] ApiSuggestionRequest request)
        {
            if (request == null || !this.ModelState.IsValid)
            { 
                return this.BasePostDelete(false);
            }

            return this.BasePostDelete(this.usersOperations.SendSuggestion(this.User.Identity.Name, request.UserId, request.TvShowId));
        }

        /// <summary>
        /// Delete the television show suggestions from user.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpDelete]
        [HawkAuthorize]
        public HttpResponseMessage Delete(string tvshowId)
        {
            return this.BasePostDelete(this.usersOperations.RemoveTvShowSuggestions(this.User.Identity.Name, tvshowId));
        }
    }
}
