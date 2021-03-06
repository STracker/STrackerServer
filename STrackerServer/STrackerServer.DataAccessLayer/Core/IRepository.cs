﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of repositories. Defines basic CRUD operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Repositories interface.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity's Id.
    /// </typeparam>
    public interface IRepository<T, in TK>
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

        /// <summary>
        /// The read all.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        ICollection<T> ReadAll();
    }
}
