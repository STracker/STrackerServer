// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachingAttribute.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Action filter for verify If-None-Match HTTP request header.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Attributes
{
    using System.Net;
    using System.Web.Http.Filters;

    /// <summary>
    /// The e tag attribute.
    /// </summary>
    public class CachingAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The on action executed.
        /// </summary>
        /// <param name="actionExecutedContext">
        /// The action executed context.
        /// </param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var etag = actionExecutedContext.Response.Headers.ETag;

            var ifNoneMatchTag = actionExecutedContext.Request.Headers.IfNoneMatch;
            if (ifNoneMatchTag.ToString().Equals(string.Empty))
            {
                // If the If-None-Match header is not defined in the request, return normally.
                return;
            }

            // Verify if the version of the entity is the same.
            if (!etag.Tag.Equals(string.Format("\"{0}\"", ifNoneMatchTag)))
            {
                // If the version is not equals, return the new one from the server.
                return;
            }

            // Else, return the status code 304 - Not modified.
            actionExecutedContext.Response.StatusCode = HttpStatusCode.MovedPermanently;
            actionExecutedContext.Response.Content = null;
        }
    }
}