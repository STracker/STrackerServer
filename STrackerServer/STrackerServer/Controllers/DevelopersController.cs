// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DevelopersController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Controller for Developers. For testing...
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// For testing.
    /// </summary>
    public class DevelopersController : Controller
    {
        /// <summary>
        /// Get the HTML view for the testing page.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Get()
        {
            return View();
        }
    }
}
