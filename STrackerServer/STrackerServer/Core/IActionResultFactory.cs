// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IActionResultFactory.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Core
{
    using System.Web.Mvc;

    /// <summary>
    /// ActionResultFactory interface.
    /// </summary>
    public interface IActionResultFactory
    {
        /// <summary>
        /// Make method.
        /// </summary>
        /// <param name="parameters">
        /// The necessary parameters for each factory.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        ActionResult Make(params object[] parameters);
    }
}