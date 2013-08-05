// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewestEpisodesRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of INewestEpisodesRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.EpisodesRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The newest episodes repository.
    /// </summary>
    public class NewestEpisodesRepository : BaseRepository<NewTvShowEpisodes, string>, INewestEpisodesRepository
    {
        /// <summary>
        /// The collection.
        /// </summary>
        private readonly MongoCollection newestCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewestEpisodesRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public NewestEpisodesRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
            this.newestCollection = this.Database.GetCollection(ConfigurationManager.AppSettings["NewestEpisodes"]);
        }

        /// <summary>
        /// The try add newest episode.
        /// </summary>
        /// <param name="synopsis">
        /// The synopsis.
        /// </param>
        /// <param name="tvshowSynopsis">
        /// The television show Synopsis.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool TryAddNewestEpisode(Episode.EpisodeSynopsis synopsis, TvShow.TvShowSynopsis tvshowSynopsis)
        {
            // Verify date.
            if (synopsis.Date == null || (DateTime.Parse(synopsis.Date) < DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd"))))
            {
                return false;
            }

            var doc = this.Read(synopsis.Id.TvshowId);
            if (doc == null)
            {
                if (!this.Create(new NewTvShowEpisodes(synopsis.Id.TvshowId) { TvShow = tvshowSynopsis }))
                {
                    return false;
                }
            }

            var query = Query<NewTvShowEpisodes>.EQ(e => e.Id, synopsis.Id.TvshowId);
            var update = Update<NewTvShowEpisodes>.Push(e => e.Episodes, synopsis);
            return this.ModifyList(this.newestCollection, query, update, this.Read(synopsis.Id.TvshowId));
        }

        /// <summary>
        /// The remove episode.
        /// </summary>
        /// <param name="synopsis">
        /// The synopsis.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveEpisode(Episode.EpisodeSynopsis synopsis)
        {
            var query = Query<NewTvShowEpisodes>.EQ(e => e.Id, synopsis.Id.TvshowId);
            var update = Update<NewTvShowEpisodes>.Pull(e => e.Episodes, synopsis);
            return this.ModifyList(this.newestCollection, query, update, this.Read(synopsis.Id.TvshowId));
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see>
        ///       <cref>ICollection</cref>
        ///     </see> .
        /// </returns>
        public ICollection<NewTvShowEpisodes> GetAll()
        {
            return this.newestCollection.FindAllAs<NewTvShowEpisodes>().ToList();
        }

        /// <summary>
        /// The Delete old episodes.
        /// </summary>
        public void DeleteOldEpisodes()
        {
            var all = this.newestCollection.FindAllAs<NewTvShowEpisodes>().ToList();
            foreach (var episodes in all)
            {
                var count = episodes.Episodes.Count;
                var oldOnes = episodes.Episodes.Where(e => DateTime.Parse(e.Date) < DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd")));
                foreach (var oldOne in oldOnes)
                {
                    if (this.RemoveEpisode(oldOne))
                    {
                        count--;
                    }
                }

                // If don't exists any new episode to show, remove the document.
                if (count <= 0)
                {
                    this.Delete(episodes.Id);
                }
            }
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(NewTvShowEpisodes entity)
        {
            this.newestCollection.Insert(entity);
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="NewTvShowEpisodes"/>.
        /// </returns>
        protected override NewTvShowEpisodes HookRead(string id)
        {
            return this.newestCollection.FindOneByIdAs<NewTvShowEpisodes>(id);
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(NewTvShowEpisodes entity)
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
            var query = Query<NewTvShowEpisodes>.EQ(e => e.Id, id);
            this.newestCollection.FindAndRemove(query, SortBy.Null);
        }
    }
}
