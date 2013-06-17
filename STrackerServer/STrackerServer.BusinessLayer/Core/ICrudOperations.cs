// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrudOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of CRUD operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// Crud operations interface.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity's Key.
    /// </typeparam>
    public interface ICrudOperations<T, in TK> where T : IEntity<TK>
    {
        /// <summary>
        /// Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Create(T entity);

        /// <summary>
        /// Read operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T Read(TK id);

        /// <summary>
        /// Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        void Update(T entity);

        /// <summary>
        /// Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        void Delete(TK id);
    }
}
