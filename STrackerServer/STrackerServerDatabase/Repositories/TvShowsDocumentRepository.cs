// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsDocumentRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Repositories
{
    using System;

    using STrackerServerDatabase.Models;

    /// <summary>
    /// DocumentRepository of television shows.
    /// </summary>
    public class TvShowsDocumentRepository : DocumentRepository<TvShow, string>
    {
        /// <summary>
        /// Create a new television show.
        /// </summary>
        /// <param name="entity">
        /// The television show.
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
        /// Get the television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public override TvShow Read(string id)
        {
            var collection = Database.GetCollection(id);

            return collection.FindOneByIdAs<TvShow>(id);
        }

        /// <summary>
        /// Update the desire television show.
        /// </summary>
        /// <param name="entity">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(TvShow entity)
        {
            var collection = Database.GetCollection(entity.Id);

            // If dont exists, creates a new one. Its a problem?!
            return collection.Save(entity).Ok;
        }

        /// <summary>
        /// Delete the television show.
        /// </summary>
        /// <param name="id">
        /// The the television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Delete(string id)
        {
            return Database.DropCollection(id).Ok;
        }
    }
}