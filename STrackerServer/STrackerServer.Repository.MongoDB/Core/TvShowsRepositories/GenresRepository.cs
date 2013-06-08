// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenresRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IGenresRepository interface.
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.TvShowsRepositories
{
    using System;
    using System.Configuration;

    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The genres repository.
    /// </summary>
    public class GenresRepository : BaseRepository<Genre, string>, IGenresRepository
    {
        /// <summary>
        /// The collection name.
        /// </summary>
        private readonly string collectionName;

        /// <summary>
        /// The collection. In this case the collection is always the same.
        /// </summary>
        private readonly MongoCollection collection;

        /// <summary>
        /// Initializes static members of the <see cref="GenresRepository"/> class.
        /// </summary>
        static GenresRepository()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(Genre)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<Genre>(
                cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(user => user.Key));
                });
        }

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
            this.collectionName = ConfigurationManager.AppSettings["GenresCollection"];
            this.collection = this.Database.GetCollection(this.collectionName);
        }

        /// <summary>
        /// Add television show synopsis to genre.
        /// </summary>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <param name="genre">
        /// The genre.
        /// </param>
        public void AddTvShowToGenre(TvShow.TvShowSynopsis tvshow, Genre.GenreSynopsis genre)
        {
            var g = this.Read(genre.Id);

            // If the genre not yet exists, create.
            if (g == null)
            {
                g = new Genre { Key = genre.Id };
                g.TvshowsSynopses.Add(tvshow);

                this.Create(g);
                return;
            }

            var query = Query<Genre>.EQ(gq => gq.Key, genre.Id.ToLower());
            var update = Update<Genre>.Push(gq => gq.TvshowsSynopses, tvshow);

            this.ModifyList(this.collection, query, update);
        }

        /// <summary>
        /// The remove television show from genre.
        /// </summary>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <param name="genre">
        /// The genre.
        /// </param>
        public void RemoveTvShowFromGenre(TvShow.TvShowSynopsis tvshow, Genre.GenreSynopsis genre)
        {
            var query = Query<Genre>.EQ(gq => gq.Key, genre.Id.ToLower());
            var update = Update<Genre>.Pull(gq => gq.TvshowsSynopses, tvshow);

            this.ModifyList(this.collection, query, update);
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(Genre entity)
        {
            entity.Key = entity.Key.ToLower();
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
            var query = Query<Genre>.EQ(g => g.Key, id.ToLower());
            this.collection.FindAndRemove(query, SortBy.Null);
        }
    }
}