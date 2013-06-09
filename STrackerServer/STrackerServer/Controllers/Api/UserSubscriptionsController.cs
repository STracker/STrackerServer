// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserSubscriptionsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for user subscriptions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Controllers.Api.AuxiliaryObjects;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The user subscriptions controller.
    /// </summary>
    public class UserSubscriptionsController : BaseController<User, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscriptionsController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        public UserSubscriptionsController(IUsersOperations operations)
            : base(operations)
        {
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        /// ApiController in Post actions only accept one argument, so the values
        /// are encapsulated inside the ApiAddUserSubscription object.
        [HttpPost]
        public HttpResponseMessage Post([FromBody] ApiAddUserSubscription value)
        {
            var state = ((IUsersOperations)this.Operations).AddSubscription(value.UserId, value.TvShowId);
            return this.BasePost(state);
        }
    }
}