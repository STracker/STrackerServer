// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdminWebController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The admin web controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// The admin web controller.
    /// </summary>
    public class AdminWebController : Controller
    {
        /// <summary>
        /// The create television show.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult CreateTvShow()
        {
            return null;
        }

        /// <summary>
        /// The create television show.
        /// </summary>
        /// <param name="form">
        /// The form.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        public ActionResult CreateTvShow(int form)
        {
            return null;
        }
    }
}
