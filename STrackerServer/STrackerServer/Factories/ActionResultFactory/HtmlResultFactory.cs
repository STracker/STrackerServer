// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlResultFactory.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Factories.ActionResultFactory
{
    using System.Web.Mvc;

    /// <summary>
    /// Factory for HTML result.
    /// </summary>
    public class HtmlResultFactory : IActionResultFactory
    {
        /// <summary>
        /// Gets Singleton object.
        /// </summary>
        public static HtmlResultFactory Singleton
        {
            get
            {
                return new HtmlResultFactory();
            }
        }

        /// <summary>
        /// Make method.
        /// </summary>
        /// <param name="parameters">
        /// The necessary parameters for HTML result factory.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Make(params object[] parameters)
        {
            return new ViewResult { ViewName = (string)parameters[1], ViewData = new ViewDataDictionary(parameters[0]) };
        }
    }
}