// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCrudOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ICrudOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// Base crud operations.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity's Key.
    /// </typeparam>
    public abstract class BaseCrudOperations<T, TK> : ICrudOperations<T, TK> where T : IEntity<TK>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCrudOperations{T,TK}"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        protected BaseCrudOperations(IRepository<T, TK> repository)
        {
            this.Repository = repository;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        protected IRepository<T, TK> Repository { get; private set; }

        /// <summary>
        /// Delete one entity.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Delete(TK id)
        {
            return this.Repository.Delete(id);
        }

        /// <summary>
        /// Create method.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract bool Create(T entity);

        /// <summary>
        /// Read method.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public abstract T Read(TK id);

        /// <summary>
        /// Update one entity.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract bool Update(T entity);
    }
}
