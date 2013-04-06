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
        /// Method for eager initialization.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResultFactory"/>.
        /// </returns>
        public static IActionResultFactory GetFactory()
        {
            return JsonResultFactoryHolder.Resource;
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

        /// <summary>
        /// Lazy initialization holder class.
        /// </summary>
        private static class JsonResultFactoryHolder
        {
            /// <summary>
            /// The value.
            /// </summary>
            private static readonly JsonResultFactory Value = new JsonResultFactory();

            /// <summary>
            /// Gets the resource.
            /// </summary>
            public static JsonResultFactory Resource
            {
                get
                {
                    return Value;
                }
            }
        }
    }
}