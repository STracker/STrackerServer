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

    using global::MongoDB.Driver;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.InformationProviders.Providers;

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
                });
            BsonClassMap.RegisterClassMap<TvShow>();

            BsonClassMap.RegisterClassMap<Person>(
                cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(c => c.Key);
                });
            BsonClassMap.RegisterClassMap<Actor>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// MongoDB client.
        /// </param>
        /// <param name="url">
        /// MongoDB url.
        /// </param>
        public TvShowsRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
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
        /// Get one television show.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public override TvShow Read(string key)
        {
            var collection = Database.GetCollection(key);

            var tvshow = collection.FindOneByIdAs<TvShow>(key);

            if (tvshow != null)
            {
                return tvshow;
            }

            var provider = new TheTvDbProvider();

            return this.TryGetFromProvider(provider.GetTvShowInformationByImdbId, key);
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
    }
}