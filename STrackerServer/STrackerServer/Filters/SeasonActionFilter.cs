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
    /// Action filter attribute for using on actions methods for seasons in television shows controller.
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

            var number = ParameterValidation(filterContext, "seasonNumber", 0);

            if (number == null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            Season season;
            TvShow.Seasons.TryGetValue((int)number, out season);
            
            if (season == null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            Season = season;
            filterContext.Controller.TempData.Add("season", Season);
        }
    }
}