// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Repositories.Implementations
{
    using STrackerServerDatabase.Models;

    /// <summary>
    /// Television shows repository for MongoDB database.
    /// </summary>
    public class TvShowsRepository : BaseRepository<TvShow, string>, ITvShowsRepository
    {
        /// <summary>
        /// Create one television show. 
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(TvShow entity)
        {
            var collection = Database.GetCollection(entity.Id);

            return collection.Insert(entity).Ok;
        }

        /// <summary>
        /// Update one television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(TvShow entity)
        {
            var collection = Database.GetCollection(entity.Id);

            return collection.Save(entity).Ok;
        }

        /// <summary>
        /// Delete one television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// This method deletes also the information about  seasons and episodes of
        /// television show.
        public override bool Delete(string id)
        {
            return Database.DropCollection(id).Ok;
        }
    }
}