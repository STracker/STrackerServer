// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.App_Start
{
    using System.Web.Mvc;

    /// <summary>
    /// Filter config.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register global filters.
        /// </summary>
        /// <param name="filters">
        /// Global Filters.
        /// </param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}