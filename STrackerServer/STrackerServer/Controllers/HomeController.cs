﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public ActionResult Index()
        {
            return this.View("Index");
        }
    }
}
