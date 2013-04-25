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

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.InformationProviders.Providers;

    /// <summary>
    /// Television shows repository for MongoDB database.
    /// </summary>
    public class TvShowsRepository : BaseRepository<TvShow, string>, ITvShowsRepository
    {
        /// <summary>
        /// The database name for television show synopsis documents.
        /// </summary>
        private const string DatabaseNameForSynopsis = "Tvshows";

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
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<TvShow> ReadAllByGenre(Genre genre)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Get one television show by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public TvShow ReadByName(string name)
        {
            var collection = Database.GetCollection<TvShow.TvShowSynopsis>(DatabaseNameForSynopsis);
            var query = Query<TvShow.TvShowSynopsis>.EQ(e => e.Name, name);
            var synopse = collection.FindOneAs<TvShow.TvShowSynopsis>(query);

            if (synopse != null)
            {
                var tvshowCollection = Database.GetCollection(synopse.Id);
                query = Query<TvShow>.EQ(tv => tv.TvShowId, synopse.Id);
                return tvshowCollection.FindOneAs<TvShow>(query);
            }

            // TODO Retrieves from external provider.
            return null;
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

            // Ensure indexes for collection
            collection.EnsureIndex(new IndexKeysBuilder().Ascending("TvShowId", "SeasonNumber", "EpisodeNumber"), IndexOptions.SetUnique(true));
            
            // Get the collection that have all references for all television shows.
            var collectionAll = Database.GetCollection(DatabaseNameForSynopsis);
            
            return collection.Insert(entity).Ok && collectionAll.Insert(entity.GetSynopsis()).Ok;
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

            var query = Query<TvShow>.EQ(tv => tv.TvShowId, key);

            var tvshow = collection.FindOneAs<TvShow>(query);

            if (tvshow != null)
            {
                tvshow.Key = key;
                return tvshow;
            }

            var provider = new TheTvDbProvider();

            return null;//this.TryGetFromProvider(provider.GetTvShowInformationByImdbId, key);
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
            var collection = Database.GetCollection(entity.TvShowId);

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