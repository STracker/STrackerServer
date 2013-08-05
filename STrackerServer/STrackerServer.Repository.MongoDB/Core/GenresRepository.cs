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
        public GenresRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
            this.collection = this.Database.GetCollection(ConfigurationManager.AppSettings["GenresCollection"]);
        }

        /// <summary>
        /// Add television show synopsis to genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddTvShowToGenre(string genre, TvShow.TvShowSynopsis tvshow)
        {
            var g = this.Read(genre);

            // If the genre not yet exists, create.
            if (g == null)
            {
                g = new Genre { Id = genre };
                g.TvshowsSynopsis.Add(tvshow);

                return this.Create(g);
            }

            var query = Query<Genre>.EQ(gq => gq.Id, genre.ToLower());
            var update = Update<Genre>.Push(gq => gq.TvshowsSynopsis, tvshow);

            return this.ModifyList(this.collection, query, update, this.Read(genre));
        }

        /// <summary>
        /// The remove television show from genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveTvShowFromGenre(string genre, TvShow.TvShowSynopsis tvshow)
        {
            var query = Query<Genre>.EQ(g => g.Id, genre.ToLower());
            var update = Update<Genre>.Pull(gq => gq.TvshowsSynopsis, tvshow);

            return this.ModifyList(this.collection, query, update, this.Read(genre));
        }

        /// <summary>
        /// Get all genres.
        /// </summary>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public ICollection<Genre.GenreSynopsis> GetAll()
        {
            return this.collection.FindAllAs<Genre>().Select(genre => genre.GetSynopsis()).ToList();
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
            throw new NotSupportedException("This method currently is not supported.");
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(string id)
        {
            var query = Query<Genre>.EQ(g => g.Id, id.ToLower());
            this.collection.FindAndRemove(query, SortBy.Null);
        }
    }
}