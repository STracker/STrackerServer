// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserWatchedEpisodesController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api controller for user's watched episodes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutUsers_Controllers
{
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The episodes watched controller.
    /// </summary>
    public class UserWatchedEpisodesController : BaseController
    {
        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserWatchedEpisodesController"/> class.
        /// </summary>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        public UserWatchedEpisodesController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
        }

        /// <summary>
        /// Add one episode to user's watched episodes list in user's subscription.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="episodeNumber">
        /// The episode number.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost]
        [HawkAuthorize]
        public HttpResponseMessage Post(string tvshowId, int seasonNumber, int episodeNumber)
        {
            return this.BasePostDelete(this.usersOperations.AddWatchedEpisode(this.User.Identity.Name, new Episode.EpisodeId { TvShowId = tvshowId, SeasonNumber = seasonNumber, EpisodeNumber = episodeNumber }));
        }

        /// <summary>
        /// Add one episode from user's watched episodes list in user's subscription.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="episodeNumber">
        /// The episode number.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpDelete]
        [HawkAuthorize]
        public HttpResponseMessage Delete(string tvshowId, int seasonNumber, int episodeNumber)
        {
            return this.BasePostDelete(this.usersOperations.RemoveWatchedEpisode(this.User.Identity.Name, new Episode.EpisodeId { TvShowId = tvshowId, SeasonNumber = seasonNumber, EpisodeNumber = episodeNumber }));
        }
    }
}