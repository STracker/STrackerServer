// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeeOtherResult.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Custom action result for see other responses.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Custom_action_results
{
    using System.Net;
    using System.Web.Mvc;

    /// <summary>
    /// The see other action result.
    /// </summary>
    public class SeeOtherResult : ActionResult 
    {
        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The execute result.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.SeeOther;
            context.HttpContext.Response.RedirectLocation = Url;
        }
    }
}