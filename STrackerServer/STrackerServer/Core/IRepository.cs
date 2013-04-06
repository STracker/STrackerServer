// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Core
{
    /// <summary>
    /// The Repository interface. CRUD operations.
    /// </summary>
    /// <typeparam name="TK">
    /// Type of id of entity.
    /// </typeparam>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    public interface IRepository<in TK, T> where T : IEntity<TK>
    {
        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Create(T entity);

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T Read(TK id);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Update(T entity);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Delete(TK id);
    }
}
