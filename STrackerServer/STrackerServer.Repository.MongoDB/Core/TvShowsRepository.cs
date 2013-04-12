// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System.Collections.Generic;

    using global::MongoDB.Driver;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Television shows repository for MongoDB database.
    /// </summary>
    public class TvShowsRepository : BaseRepository<TvShow, string>, ITvShowsRepository
    {
        /// <summary>
        /// The get all by genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public List<TvShow> GetAllByGenre(Genre genre)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Implementation of Create hook method.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool Create(TvShow entity, MongoCollection collection)
        {
            return collection.Insert(entity).Ok;
        }

        /// <summary>
        /// Implementation of Update hook method.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool Update(TvShow entity, MongoCollection collection)
        {
            return collection.Save(entity).Ok;
        }

        /// <summary>
        /// Implementation of Delete hook method.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// This method deletes also the information about  seasons and episodes of
        /// television show.
        protected override bool Delete(string id, MongoCollection collection)
        {
            return this.Database.DropCollection(id).Ok;
        }

        /// <summary>
        /// Implementation of get document collection hook method.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="MongoCollection"/>.
        /// </returns>
        protected override MongoCollection GetDocumentCollection(string id)
        {
            return Database.GetCollection(id);
        }
    }
}