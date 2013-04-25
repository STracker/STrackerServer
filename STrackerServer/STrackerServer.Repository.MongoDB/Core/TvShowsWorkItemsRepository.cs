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

    using global::MongoDB.Driver;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The work for create television shows repository.
    /// </summary>
    public class TvShowsWorkItemsRepository : BaseRepository<TvShowWorkItem, string>, ITvShowsWorkItemsRepository
    {
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}