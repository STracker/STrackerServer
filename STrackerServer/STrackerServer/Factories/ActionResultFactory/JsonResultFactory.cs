// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonResultFactory.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Factories.ActionResultFactory
{
    using System.Web.Mvc;

    using STrackerServer.Core;

    /// <summary>
    /// Factory for JSON result.
    /// </summary>
    public class JsonResultFactory : IActionResultFactory
    {
        /// <summary>
        /// The value.
        /// </summary>
        private static readonly JsonResultFactory Value = new JsonResultFactory();

        /// <summary>
        /// Gets the singleton.
        /// </summary>
        public static JsonResultFactory Singleton
        {
            get
            {
                return Value;
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