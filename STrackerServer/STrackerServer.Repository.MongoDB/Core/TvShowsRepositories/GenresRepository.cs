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
        private const string CollectionName = "TvShows-Genres";

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
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public override void HookCreate(Genre entity)
        {
            var collection = this.Database.GetCollection(CollectionName);
            entity.Id = entity.Id.ToLower();
            collection.Insert(entity);
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
        public override Genre HookRead(string id)
        {
            var collection = this.Database.GetCollection(CollectionName);
            return collection.FindOneByIdAs<Genre>(id.ToLower());
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public override void HookUpdate(Genre entity)
        {
            throw new NotSupportedException("this method currently is not supported.");
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public override void HookDelete(string id)
        {
            var collection = this.Database.GetCollection(CollectionName);
            var query = Query<Genre>.EQ(g => g.Id, id.ToLower());
            collection.FindAndRemove(query, SortBy.Null);
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
            var g = this.Read(genre.GenreType);
            
            // If the genre not yet exists, create.
            if (g == null)
            {
                g = new Genre { Id = genre.GenreType };
                g.TvshowsSynopses.Add(tvshow);

                this.Create(g);
                return;
            }

            var collection = this.Database.GetCollection(CollectionName);
            var query = Query<Genre>.EQ(gq => gq.Id, genre.GenreType.ToLower());
            collection.FindAndModify(query, SortBy.Null, Update<Genre>.Push(gq => gq.TvshowsSynopses, tvshow));
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
            var g = this.Read(genre.GenreType);
            if (g == null)
            {
                return;
            }

            var collection = this.Database.GetCollection(CollectionName);
            var query = Query<Genre>.EQ(gq => gq.Id, genre.GenreType.ToLower());
            collection.FindAndModify(query, SortBy.Null, Update<Genre>.Pull(gq => gq.TvshowsSynopses, tvshow));
        }
    }
}
