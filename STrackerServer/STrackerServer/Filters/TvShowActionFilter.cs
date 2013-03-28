﻿// --------------------------------------------------------------------------------------------------------------------
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
    /// Action filter attribute for using on actions methods for television shows in television shows controller.
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

            var id = ParameterValidation(filterContext, "tvshowId", 0);

            if (id == null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            if ((TvShow = repository.Get((int)id)) != null)
            {
                return;
            }

            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }

        /// <summary>
        /// Checks if the action parameter is correct.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        /// <param name="parameterName">
        /// The action parameter name.
        /// </param>
        /// <param name="defaultValue">
        /// The default value of the action parameter type.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        protected object ParameterValidation(ActionExecutingContext filterContext, string parameterName, object defaultValue)
        {
            object param;
            filterContext.ActionParameters.TryGetValue(parameterName, out param);

            if (param == null)
            {
                filterContext.ActionParameters.Remove(parameterName);
                filterContext.ActionParameters.Add(parameterName, defaultValue);
            }

            return param;
        }
    }
}