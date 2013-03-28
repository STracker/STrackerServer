// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonResultFactory.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Factories.ActionResultFactory
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Factory for JSON result.
    /// </summary>
    public class JsonResultFactory : IActionResultFactory
    {
        /// <summary>
        /// Gets Singleton object.
        /// </summary>
        public static JsonResultFactory Singleton
        {
            get
            {
                return new JsonResultFactory();  
            }
        }

        /// <summary>
        /// Make method.
        /// </summary>
        /// <param name="parameters">
        /// The necessary parameters for JSON result factory.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Make(params object[] parameters)
        {
            return new JsonResult { Data = parameters[0], JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}