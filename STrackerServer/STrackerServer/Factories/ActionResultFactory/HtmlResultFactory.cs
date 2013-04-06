// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlResultFactory.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Factories.ActionResultFactory
{
    using System.Web.Mvc;

    using STrackerServer.Core;

    /// <summary>
    /// Factory for HTML result.
    /// </summary>
    public class HtmlResultFactory : IActionResultFactory
    {
        /// <summary>
        /// Method for eager initialization.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResultFactory"/>.
        /// </returns>
        public static IActionResultFactory GetFactory()
        {
            return HtmlResultFactoryHolder.Resource;
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

        /// <summary>
        /// Lazy initialization holder class.
        /// </summary>
        private static class HtmlResultFactoryHolder
        {
            /// <summary>
            /// The value.
            /// </summary>
            private static readonly HtmlResultFactory Value = new HtmlResultFactory();

            /// <summary>
            /// Gets the resource.
            /// </summary>
            public static HtmlResultFactory Resource
            {
                get
                {
                    return Value;
                }
            }
        }
    }
}