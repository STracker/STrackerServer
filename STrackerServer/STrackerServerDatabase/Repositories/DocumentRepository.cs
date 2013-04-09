// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Repositories
{
    using MongoDB.Driver;

    using STrackerServerDatabase.Core;

    /// <summary>
    /// The base repository. Contains the MongoDB collection that have the entities associated to repository.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TE">
    /// Type of id of entity.
    /// </typeparam>
    public abstract class DocumentRepository<T, TE> : IRepository<string, T, TE> where T : IEntity<string>
    {
        /// <summary>
        /// The database.
        /// </summary>
        protected readonly MongoDatabase Database;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentRepository{T,TE}"/> class.
        /// </summary>
        protected DocumentRepository()
        {
            this.Database = DatabaseLocator.GetMongoDbDatabaseInstance();
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract bool Create(T entity);

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public abstract T Read(TE id);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract bool Update(T entity);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract bool Delete(TE id);
    }
}