// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonActionFilter.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Filters
{
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Models.Media;

    /// <summary>
    /// Action filter attribute for using on actions methods in seasons controller.
    /// </summary>
    public class SeasonActionFilter : TvShowActionFilter
    {
        /// <summary>
        /// Gets or sets the season.
        /// </summary>
        protected Season Season { get; set; }

        /// <summary>
        /// Action attribute that is executed after the action. Checks if the season with the same Number exists.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (TvShow == null)
            {
                return;
            }

            var number = filterContext.ActionParameters["seasonNumber"];

            if (number == null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            if ((Season = TvShow.Seasons.ToArray()[(int)number]) == null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            filterContext.Controller.TempData.Add("season", Season);
        }
    }
}