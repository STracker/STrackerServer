// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseWebController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Base Web Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using BusinessLayer.Core;

    /// <summary>
    /// The base web controller.
    /// </summary>
    public abstract class BaseWebController : Controller
    {
        /// <summary>
        /// The get view.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        protected ActionResult GetView(OperationResultState state)
        {
            switch (state)
            {
                case OperationResultState.InProcess:
                    Response.StatusCode = (int)HttpStatusCode.Accepted;
                    return this.View("Error", Response.StatusCode);

                case OperationResultState.NotFound:
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return this.View("Error", Response.StatusCode);

                default:
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return this.View("Error", Response.StatusCode);
            }  
        }
    }
}
