// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionResultFilter.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Filters
{
    using System.Web.Mvc;

    /// <summary>
    /// Action filter attribute for using on actions methods for getting the correct response content-type.
    /// </summary>
    public class ActionResultFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Action attribute that is executed after the action. Checks the content-type and add on TempData object from controller.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            filterContext.Controller.TempData.Add("formatType", filterContext.RequestContext.RouteData.GetRequiredString("format"));
        }
    }
}