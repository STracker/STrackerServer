// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The system controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System;
    using System.Web.Http;

    using STrackerServer.Controllers.Api.AuxiliaryObjects;

    /// <summary>
    /// The system controller.
    /// </summary>
    public class SystemController : ApiController
    {
        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        [System.Web.Http.HttpGet]
        public TimeUtc GetTime()
        {
            return new TimeUtc { Time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") };
        }
    }
}
