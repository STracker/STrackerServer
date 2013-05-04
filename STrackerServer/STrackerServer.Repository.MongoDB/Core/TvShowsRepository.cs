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
    using System;
    using System.Collections.Generic;

    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

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
        /// Initializes static members of the <see cref="TvShowsRepository"/> class.
        /// </summary>
        static TvShowsRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Person)))
            {
                BsonClassMap.RegisterClassMap<Person>(
                cm =>
                {
                    cm.AutoMap();

                    // map _id field to key property.
                    cm.SetIdMember(cm.GetMemberMap(p => p.Key));
                });
                BsonClassMap.RegisterClassMap<Actor>();
                BsonClassMap.RegisterClassMap<User>();
            }

            if (BsonClassMap.IsClassMapRegistered(typeof(Media)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<Media>(
                cm =>
                    {
                        cm.AutoMap();
                        cm.UnmapProperty(c => c.Key);

                        // ignoring _id field when deserialize.
                        cm.SetIgnoreExtraElementsIsInherited(true);
                        cm.SetIgnoreExtraElements(true);
                    });
            BsonClassMap.RegisterClassMap<TvShow>();
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
        /// Create one television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookCreate(TvShow entity)
        {
            var collection = Database.GetCollection(entity.Key);

            // Get the collection that have all references for all television shows.
            var collectionAll = Database.GetCollection(DatabaseNameForSynopsis);

            // Ensure indexes for collections
            collection.EnsureIndex(new IndexKeysBuilder().Ascending("TvShowId", "SeasonNumber", "EpisodeNumber"), IndexOptions.SetUnique(true));
            collectionAll.EnsureIndex(new IndexKeysBuilder().Ascending("Name"));

            // The order is relevant because mongo don't ensure transactions.
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
        public override TvShow HookRead(string key)
        {
            var collection = Database.GetCollection(key);
            var query = Query<TvShow>.EQ(tv => tv.TvShowId, key);
            
            var tvshow = collection.FindOneAs<TvShow>(query);
            if (tvshow == null)
            {
                return null;
            }

            tvshow.Key = key;
            return tvshow;
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
        public override bool HookUpdate(TvShow entity)
        {
            var collection = Database.GetCollection(entity.TvShowId);
            var query = Query<TvShow>.EQ(tv => tv.TvShowId, entity.TvShowId);
            var update = Update<TvShow>.Set(tv => tv.SeasonSynopses, entity.SeasonSynopses).Set(tv => tv.Rating, entity.Rating).Set(tv => tv.Runtime, entity.Runtime);

            return collection.Update(query, update).Ok;
        }

        /// <summary>
        /// Delete one television show.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// This method also deletes the information about seasons and episodes of
        /// television show.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookDelete(string key)
        {
            return this.Database.DropCollection(key).Ok;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// The read by name.
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

            try
            {
                var synopse = collection.FindOneAs<TvShow.TvShowSynopsis>(query);
                if (synopse == null)
                {
                    return null;
                }

                var tvshowCollection = Database.GetCollection(synopse.Id);
                query = Query<TvShow>.EQ(tv => tv.TvShowId, synopse.Id);
                return tvshowCollection.FindOneAs<TvShow>(query);
            }
            catch (Exception)
            {
                // TODO, add exception to Log mechanism.
                return null;
            }
        }
    }
}