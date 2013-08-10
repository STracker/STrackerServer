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
    using System.Collections.Generic;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// Base crud operations.
    /// </summary>
    /// <typeparam name="TI">
    /// The interface of the property Repository.
    /// </typeparam>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity's Id.
    /// </typeparam>
    public abstract class BaseCrudOperations<TI, T, TK> : ICrudOperations<T, TK> where T : IEntity<TK> where TI : IRepository<T, TK>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCrudOperations{TI,T,TK}"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        protected BaseCrudOperations(TI repository)
        {
            this.Repository = repository;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        protected TI Repository { get; private set; }

        /// <summary>
        /// Create method.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Don't need to validate fields, because only administrator can use this 
        /// operation.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Create(T entity)
        {
            return this.Repository.Create(entity);
        }

        /// <summary>
        /// The read.
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
        /// Don't need to validate fields, because only administrator can use this 
        /// operation.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Update(T entity)
        {
            return this.Repository.Update(entity);
        }

        /// <summary>
        /// Delete one entity.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// Don't need to validate fields, because only administrator can use this 
        /// operation.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Delete(TK id)
        {
            return this.Repository.Delete(id);
        }

        /// <summary>
        /// The read all.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<T> ReadAll()
        {
            return this.Repository.ReadAll();
        }
    }
}