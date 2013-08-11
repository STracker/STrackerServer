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
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;

    using STrackerServer.DataAccessLayer.Core;

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
        protected HttpResponseMessage BaseGet<TT>(TT obj)
        {
            if (Equals(obj, default(TT)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        /// <summary>
        /// Auxiliary methods for Get operations over entities domains objects.
        /// Sets the HTTP ETag header value for caching proposes in Clients.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <typeparam name="T">
        /// The type of the entity id.
        /// </typeparam>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        protected HttpResponseMessage BaseGetForEntities<T>(IEntity<T> entity)
        {
            if (Equals(entity, default(IEntity<T>)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            
            // Create response and set ETag header value.
            var response = this.Request.CreateResponse(HttpStatusCode.OK, entity);
            response.Headers.ETag = new EntityTagHeaderValue(string.Format("\"{0}\"", entity.Version));
            return response;
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
        protected HttpResponseMessage BasePostDelete(bool state)
        {
            return this.Request.CreateResponse(state ? HttpStatusCode.OK : HttpStatusCode.BadRequest, new { Status = state });
        }
    }
}