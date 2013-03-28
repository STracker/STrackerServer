// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Factories.ActionResultFactory;
    using STrackerServer.Filters;

    /// <summary>
    /// Base controller.
    /// </summary>
    [ActionResultFilter]
    public class BaseController : Controller
    {
        /// <summary>
        /// Action results factories.
        /// </summary>
        protected readonly IDictionary<string, IActionResultFactory> Factories;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        public BaseController()
        {
            Factories = new Dictionary<string, IActionResultFactory>
                {
                    { "json", JsonResultFactory.Singleton },
                    { "html", HtmlResultFactory.Singleton } 
                };
        }

        /// <summary>
        /// Base method for get actions.
        /// </summary>
        /// <param name="objectType">
        /// The object type.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        protected ActionResult BaseGet(string objectType)
        {
            var factory = Factories[(string)TempData["formatType"]];
            var model = TempData[objectType];

            TempData.Clear();

            return (Response.StatusCode != (int)HttpStatusCode.OK)
                       ? factory.Make(Response.StatusCode, "Error")
                       : factory.Make(model, "Get");
        }
    }
}
