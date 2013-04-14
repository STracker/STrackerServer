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
        protected readonly ICrudOperations<T, TK> Operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{T,TK}"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        protected BaseController(ICrudOperations<T, TK> operations)
        {
            this.Operations = operations;
        }

        /// <summary>
        /// Auxiliary method for Get operations.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        protected T Get(T entity)
        {
            if (Equals(entity, default(T)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return entity;
        }
    }
}
