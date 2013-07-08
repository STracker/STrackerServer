// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Base Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// Base controller.
    /// </summary>
    public abstract class BaseController : ApiController
    {
        /// <summary>
        /// Auxiliary method for Get operations.
        /// </summary>
        /// <typeparam name="TT">
        /// Type of the object.
        /// </typeparam>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        protected HttpResponseMessage BaseGet<TT>(TT obj)
        {
            if (Equals(obj, default(TT)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        /// <summary>
        /// Auxiliary method for Post operations.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        protected HttpResponseMessage BasePost(bool state)
        {
            return this.Request.CreateResponse(state ? HttpStatusCode.OK : HttpStatusCode.BadRequest, new { Status = state });
        }
    }
}