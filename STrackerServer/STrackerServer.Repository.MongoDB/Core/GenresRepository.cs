// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenresRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IGenresRepository interface.
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Logger.Core;

    /// <summary>
    /// The genres repository.
    /// </summary>
    public class GenresRepository : BaseRepository<Genre, string>, IGenresRepository
    {
        /// <summary>
        /// The collection. In this case the collection is always the same.
        /// </summary>
        private readonly MongoCollection collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenresRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public GenresRepository(MongoClient client, MongoUrl url, ILogger logger)
            : base(client, url, logger)
        {
            this.collection = this.Database.GetCollection(ConfigurationManager.AppSettings["GenresCollection"]);
        }

        /// <summary>
        /// Add one television show to genre.
        /// </summary>
        /// <param name="genre">
        /// The genre name.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddTvShow(string genre, TvShow.TvShowSynopsis tvshow)
        {
            var genreDoc = this.Read(genre);

            // If the genre not yet exists, create.
            if (genreDoc == null)
            {
                genreDoc = new Genre(genre);
                genreDoc.Tvshows.Add(tvshow);

                return this.Create(genreDoc);
            }

            var query = Query<Genre>.EQ(gq => gq.Id, genre.ToLower());
            var update = Update<Genre>.AddToSet(gq => gq.Tvshows, tvshow);
            return this.ModifyList(this.collection, query, update, genreDoc);
        }

        /// <summary>
        /// Remove one television show from genre.
        /// </summary>
        /// <param name="genre">
        /// The genre name.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveTvShow(string genre, TvShow.TvShowSynopsis tvshow)
        {
            var query = Query<Genre>.EQ(g => g.Id, genre.ToLower());
            var update = Update<Genre>.Pull(g => g.Tvshows, tvshow);
            return this.ModifyList(this.collection, query, update, this.Read(genre));
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(Genre entity)
        {
            entity.Id = entity.Id.ToLower();
            this.collection.Insert(entity);
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Genre"/>.
        /// </returns>
        protected override Genre HookRead(string id)
        {
            return this.collection.FindOneByIdAs<Genre>(id.ToLower());
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(Genre entity)
        {
            throw new NotSupportedException("this method currently is not supported.");
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(string id)
        {
            throw new NotSupportedException("this method currently is not supported.");
        }

        /// <summary>
        /// Hook method for Read all operation.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        protected override ICollection<Genre> HookReadAll()
        {
            return this.collection.FindAllAs<Genre>().ToList();
        }
    }
}