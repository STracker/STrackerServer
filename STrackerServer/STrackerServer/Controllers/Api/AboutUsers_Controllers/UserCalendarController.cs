// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserCalendarController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api controller for user's episodes calendar.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutUsers_Controllers
{
    using System;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.UsersOperations;

    /// <summary>
    /// The user calendar controller.
    /// </summary>
    public class UserCalendarController : BaseController
    {
        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCalendarController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        public UserCalendarController(IUsersOperations operations)
        {
            this.operations = operations;
        }

        /// <summary>
        /// Get the user's calendar for the next 7 days.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [HawkAuthorize]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.operations.GetUserNewEpisodes(this.User.Identity.Name, DateTime.UtcNow.AddDays(7).ToString("yyyy-MM-dd")));
        }
    }
}