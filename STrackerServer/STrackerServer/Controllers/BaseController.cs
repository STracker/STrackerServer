// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using STrackerServer.Core;
    using STrackerServer.Factories.ActionResultFactory;

    /// <summary>
    /// The base controller.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// The representations.
        /// </summary>
        private readonly IDictionary<string, IActionResultFactory> representations;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        public BaseController()
        {
            representations = new Dictionary<string, IActionResultFactory>
                {
                    { "html", HtmlResultFactory.Singleton }, { "json", JsonResultFactory.Singleton } 
                };
        }

        /// <summary>
        /// Gets the representations.
        /// </summary>
        protected IDictionary<string, IActionResultFactory> Representations
        {
            get
            {
                return representations;
            }
        } 
    }
}
