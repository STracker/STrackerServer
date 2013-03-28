// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowActionFilter.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Filters
{
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Models.Media;
    using STrackerServer.Testing_Repository;

    /// <summary>
    /// Action filter attribute for using on actions methods in television shows controller.
    /// </summary>
    public class TvShowActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Television shows repository.
        /// </summary>
        private readonly TvShowsTestRepository repository = TestRepositoryLocator.TvShowsTestRepo;

        /// <summary>
        /// Gets or sets the television show.
        /// </summary>
        protected TvShow TvShow { get; set; }

        /// <summary>
        /// Action attribute that is executed after the action. Checks if the television show with the same Id exists.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var id = filterContext.ActionParameters["tvshowId"];

            if (id == null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            if ((TvShow = repository.Get((string)id)) == null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            filterContext.Controller.TempData.Add("tvshow", TvShow);
        }
    }
}