// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTvShowWorksRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ICreateTvShowWorksRepository interface. This repository connects with 
//  MongoDB database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using global::MongoDB.Bson.Serialization;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.InformationProviders.Core;

    /// <summary>
    /// The create television show works repository.
    /// </summary>
    public class CreateTvShowWorksRepository : BaseRepository<CreateTvShowWork, string>, ICreateTvShowWorksRepository
    {
        /// <summary>
        /// The collection.
        /// </summary>
        private readonly MongoCollection collection;

        /// <summary>
        /// The info provider.
        /// </summary>
        private readonly ITvShowsInformationProvider infoProvider;

        /// <summary>
        /// The television show repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// The seasons repository.
        /// </summary>
        private readonly ISeasonsRepository seasonsRepository;

        /// <summary>
        /// The episodes repository.
        /// </summary>
        private readonly IEpisodesRepository episodesRepository;

        /// <summary>
        /// Initializes static members of the <see cref="CreateTvShowWorksRepository"/> class.
        /// </summary>
        static CreateTvShowWorksRepository()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(CreateTvShowWork)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<CreateTvShowWork>(
                cm =>
                    {
                        cm.AutoMap();
                        cm.SetIdMember(cm.GetMemberMap(c => c.Key));
                    });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTvShowWorksRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="infoProvider">
        /// The info Provider.
        /// </param>
        /// <param name="tvshowsRepository">
        /// The television shows Repository.
        /// </param>
        /// <param name="seasonsRepository">
        /// The seasons Repository.
        /// </param>
        /// <param name="episodesRepository">
        /// The episodes Repository.
        /// </param>
        public CreateTvShowWorksRepository(MongoClient client, MongoUrl url, ITvShowsInformationProvider infoProvider, ITvShowsRepository tvshowsRepository, ISeasonsRepository seasonsRepository, IEpisodesRepository episodesRepository)
            : base(client, url)
        {
            collection = Database.GetCollection<CreateTvShowWork>("CreateTvShowWorkQueue");
            this.infoProvider = infoProvider;
            this.tvshowsRepository = tvshowsRepository;
            this.seasonsRepository = seasonsRepository;
            this.episodesRepository = episodesRepository;
        }

        /// <summary>
        /// Create one work.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookCreate(CreateTvShowWork entity)
        {
            // Verify if the tv show really exists.
            if (!this.infoProvider.VerifyIfExists(entity.Key))
            {
                throw new InvalidIdException();
            }

            var task = Task.Factory.StartNew(() => this.ExecuteCreateTask(entity.Key));
            task.ContinueWith(
                completed =>
                    {
                        completed.Wait();
                        this.Delete(entity.Key);
                    });

            return this.collection.Insert(entity).Ok;
        }

        /// <summary>
        /// Get one work.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="CreateTvShowWork"/>.
        /// </returns>
        public override CreateTvShowWork HookRead(string key)
        {
            return this.collection.FindOneByIdAs<CreateTvShowWork>(key);
        }

        /// <summary>
        /// Update one work.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookUpdate(CreateTvShowWork entity)
        {
            // Don't need to update, all fields are unique.
            return true;
        }

        /// <summary>
        /// Delete one work.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookDelete(string key)
        {
            var query = Query<CreateTvShowWork>.EQ(c => c.Key, key);
            return this.collection.FindAndRemove(query, SortBy.Null).Ok;
        }

        /// <summary>
        /// The execute create task method.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        private void ExecuteCreateTask(string id)
        {
            var tvshowInfo = this.infoProvider.GetTvShowInformation(id);
            if (tvshowInfo == null || !this.tvshowsRepository.Create(tvshowInfo))
            {
                return;
            }

            var seasonsInfo = this.infoProvider.GetSeasonsInformation(id);
            var enumerable = seasonsInfo as List<Season> ?? seasonsInfo.ToList();
            if (seasonsInfo == null || !this.seasonsRepository.CreateAll(enumerable))
            {
                return;
            }

            foreach (var episodesInfo in enumerable.Select(season => this.infoProvider.GetEpisodesInformation(id, season.SeasonNumber)).Where(episodesInfo => episodesInfo != null))
            {
                this.episodesRepository.CreateAll(episodesInfo);
            }
        }
    }
}