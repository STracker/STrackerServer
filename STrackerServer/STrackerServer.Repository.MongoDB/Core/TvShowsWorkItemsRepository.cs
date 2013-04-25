// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsWorkItemsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsWorkItemsRepository interface. This repository connects  
//  with MongoDB database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System;

    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    using global::MongoDB.Driver.Builders;

    /// <summary>
    /// The work for create television shows repository.
    /// </summary>
    public class TvShowsWorkItemsRepository : BaseRepository<TvShowWorkItem, string>, ITvShowsWorkItemsRepository
    {
        /// <summary>
        /// The collection.
        /// </summary>
        private readonly MongoCollection<TvShowWorkItem> collection;

        /// <summary>
        /// Initializes static members of the <see cref="TvShowsWorkItemsRepository"/> class.
        /// </summary>
        static TvShowsWorkItemsRepository()
        {
            BsonClassMap.RegisterClassMap<TvShowWorkItem>(
                cm =>
                {
                    cm.AutoMap();

                    // map _id field to key property.
                    cm.SetIdMember(cm.GetMemberMap(p => p.Key));
                });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsWorkItemsRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public TvShowsWorkItemsRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
            this.collection = this.Database.GetCollection<TvShowWorkItem>("WorkItemsQueue");
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="TvShowWorkItem"/>.
        /// </returns>
        public override TvShowWorkItem Read(string key)
        {
            return this.collection.FindOneById(key);
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(TvShowWorkItem entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Delete(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(TvShowWorkItem entity)
        {
            return this.collection.Insert(entity).Ok;
        }
    }
}