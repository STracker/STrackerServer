// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeActionFilter.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Filters.Media_Filters
{
    using System.Net;

    /// <summary>
    /// Action filter attribute for using on actions methods in episodes controller.
    /// </summary>
    public class EpisodeActionFilter : SeasonActionFilter
    {
        /// <summary>
        /// Action attribute that is executed after the action. Checks if the episode with the same Number exists.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Season == null)
            {
                return;
            }

            var number = filterContext.ActionParameters["episodeNumber"];

            if (number == null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            var episode = Season.Episodes.ToArray()[(int)number];

            if (episode == null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            filterContext.Controller.TempData.Add("episode", episode);
        }
    }
}