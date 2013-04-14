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

    using global::MongoDB.Bson.Serialization;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Television shows repository for MongoDB database.
    /// </summary>
    public class TvShowsRepository : BaseRepository<TvShow, string>, ITvShowsRepository
    {
        /// <summary>
        /// Initializes static members of the <see cref="TvShowsRepository"/> class.
        /// </summary>
        static TvShowsRepository()
        {
            BsonClassMap.RegisterClassMap<Media>(
                cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(c => c.Key);
                    cm.GetMemberMap(c => c.Actors).SetIgnoreIfNull(true);
                    cm.GetMemberMap(c => c.Artworks).SetIgnoreIfNull(true);
                });
            BsonClassMap.RegisterClassMap<TvShow>(
                cm =>
                    {
                        cm.AutoMap();
                        cm.GetMemberMap(c => c.SeasonSynopses).SetIgnoreIfNull(true);
                    });

            BsonClassMap.RegisterClassMap<Person>(
                cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(c => c.Key);
                    cm.GetMemberMap(c => c.Photo).SetIgnoreIfNull(true);
                });
            BsonClassMap.RegisterClassMap<Actor>();
        }

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
            var collection = Database.GetCollection(entity.Key);

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
            var collection = Database.GetCollection(entity.Key);

            return collection.Save(entity).Ok;
        }

        /// <summary>
        /// Delete one television show.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// This method also deletes the information about  seasons and episodes of
        /// television show.
        public override bool Delete(string key)
        {
            return this.Database.DropCollection(key).Ok;
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        protected override TvShow HookRead(string key)
        {
            var collection = Database.GetCollection(key);

            return collection.FindOneByIdAs<TvShow>(key);
        }
    }
}