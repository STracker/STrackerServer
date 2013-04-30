// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Base Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// Base controller.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity's Key.
    /// </typeparam>
    public abstract class BaseController<T, TK> : ApiController where T : IEntity<TK>
    {
        /// <summary>
        /// The operations.
        /// </summary>
        protected readonly IAsyncOperations<T, TK> Operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{T,TK}"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        protected BaseController(IAsyncOperations<T, TK> operations)
        {
            this.Operations = operations;
        }

        /// <summary>
        /// Auxiliary method for Get operations.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        protected HttpResponseMessage TryGet(T entity, OperationResultState state)
        {
            switch (state)
            {
                    case OperationResultState.InProcess:
                        return Request.CreateResponse(HttpStatusCode.Accepted, "in process...");

                    case OperationResultState.NotFound:
                        throw new HttpResponseException(HttpStatusCode.NotFound);

                    case OperationResultState.Error:
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);

                    default:
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
        }
    }
}
