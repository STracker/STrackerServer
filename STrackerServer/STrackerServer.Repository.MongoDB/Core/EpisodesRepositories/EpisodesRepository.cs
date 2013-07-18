// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.EpisodesRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using global::MongoDB.Bson.Serialization;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// Episodes repository for MongoDB database.
    /// </summary>
    public class EpisodesRepository : BaseRepository<Episode, Tuple<string, int, int>>, IEpisodesRepository
    {
        /// <summary>
        /// The collection.
        /// </summary>
        private readonly MongoCollection newestCollection;

        /// <summary>
        /// Seasons repository.
        /// </summary>
        private readonly ISeasonsRepository seasonsRepository;

        /// <summary>
        /// The comments repository.
        /// </summary>
        private readonly IEpisodeCommentsRepository commentsRepository;

        /// <summary>
        /// The ratings repository.
        /// </summary>
        private readonly IEpisodeRatingsRepository ratingsRepository;

        /// <summary>
        /// Initializes static members of the <see cref="EpisodesRepository"/> class.
        /// </summary>
        static EpisodesRepository()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(NewestEpisodes)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<NewestEpisodes>(
                cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(user => user.Key));
                });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesRepository"/> class.
        /// </summary>
        /// <param name="seasonsRepository">
        /// The seasons repository.
        /// </param>
        /// <param name="commentsRepository">
        /// The comments Repository.
        /// </param>
        /// <param name="ratingsRepository">
        /// The ratings Repository.
        /// </param>
        /// <param name="client">
        /// MongoDB client.
        /// </param>
        /// <param name="url">
        /// MongoDB url.
        /// </param>
        public EpisodesRepository(ISeasonsRepository seasonsRepository, IEpisodeCommentsRepository commentsRepository, IEpisodeRatingsRepository ratingsRepository, MongoClient client, MongoUrl url) 
            : base(client, url)
        {
            this.seasonsRepository = seasonsRepository;
            this.commentsRepository = commentsRepository;
            this.ratingsRepository = ratingsRepository;

            this.newestCollection = this.Database.GetCollection(ConfigurationManager.AppSettings["NewestEpisodes"]);
        }

        /// <summary>
        /// Create several episodes.
        /// </summary>
        /// <param name="episodes">
        /// The episodes.
        /// </param>
        public void CreateAll(IEnumerable<Episode> episodes)
        {
            var enumerable = episodes as List<Episode> ?? episodes.ToList();
            if (!enumerable.Any())
            {
                return;
            }

            foreach (var episode in enumerable)
            {
                this.Create(episode);
            }
        }

        /// <summary>
        /// The get all from one season.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<Episode.EpisodeSynopsis> GetAllFromOneSeason(string tvshowId, int seasonNumber)
        {
            return this.seasonsRepository.Read(new Tuple<string, int>(tvshowId, seasonNumber)).EpisodeSynopses;
        }

        /// <summary>
        /// The get newest episodes.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<Episode.EpisodeSynopsis> GetNewestEpisodes(string tvshowId)
        {
            try
            {
                return this.newestCollection.FindOneByIdAs<NewestEpisodes>(tvshowId).Episodes;
            }
            catch (Exception)
            {
                // TODO, add to log mechanism.
                return null;
            }
        }

        /// <summary>
        /// The Delete old episodes.
        /// </summary>
        public void DeleteOldEpisodes()
        {
            try
            {
                var all = this.newestCollection.FindAllAs<NewestEpisodes>().ToList();
                foreach (var episodes in all)
                {
                    var count = episodes.Episodes.Count;

                    var oldOnes = episodes.Episodes.Where(e => DateTime.Parse(e.Date) < DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd")));
                    foreach (var oldOne in oldOnes)
                    {
                        var query = Query<NewestEpisodes>.EQ(we => we.Key, episodes.Key);
                        var update = Update<NewestEpisodes>.Pull(we => we.Episodes, oldOne);
                        this.ModifyList(this.newestCollection, query, update);

                        count--;
                    }

                    if (count > 0)
                    {
                        continue;
                    }

                    // If don't exists any new episode to show, remove the document.
                    var query2 = Query<NewestEpisodes>.EQ(we => we.Key, episodes.Key);
                    this.newestCollection.FindAndRemove(query2, SortBy.Null);
                }
            }
            catch (Exception)
            {
                // TODO, add to log mechanism.
            }
        }

        /// <summary>
        /// The get newest episodes.
        /// </summary>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<Episode.EpisodeSynopsis> GetNewestEpisodes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create one episode.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Needs to create also the object synopse in season episodes list.
        protected override void HookCreate(Episode entity)
        {
            var collection = this.Database.GetCollection(entity.TvShowId);
            collection.Insert(entity);

            // Add the synopse of the entity to season.
            this.seasonsRepository.AddEpisodeSynopsis(entity.TvShowId, entity.SeasonNumber, entity.GetSynopsis());

            // Try add to newest document.
            this.TryAddToNewestDocument(entity);

            // Also create the documents for comments and ratings.
            this.commentsRepository.Create(new CommentsEpisode(entity.Key));
            this.ratingsRepository.Create(new RatingsEpisode(entity.Key));
        }

        /// <summary>
        /// Get one episode.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="STrackerServer.DataAccessLayer.DomainEntities.Episode"/>.
        /// </returns>
        protected override Episode HookRead(Tuple<string, int, int> id)
        {
            var collection = this.Database.GetCollection(id.Item1);
            var query = Query.And(Query<Episode>.EQ(e => e.TvShowId, id.Item1), Query<Episode>.EQ(e => e.SeasonNumber, id.Item2), Query<Episode>.EQ(e => e.EpisodeNumber, id.Item3));
            return collection.FindOne<Episode>(query, "_id");
        }

        /// <summary>
        /// Update one episode.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Needs to update also the object synopse in season episodes list.
        protected override void HookUpdate(Episode entity)
        {
            // Try add to newest document.
            this.TryAddToNewestDocument(entity);

            var collection = this.Database.GetCollection(entity.TvShowId);

            var query = Query.And(Query<Episode>.EQ(e => e.TvShowId, entity.TvShowId), Query<Episode>.EQ(e => e.SeasonNumber, entity.SeasonNumber), Query<Episode>.EQ(e => e.EpisodeNumber, entity.EpisodeNumber));
            var update = Update<Episode>.Set(e => e.SeasonNumber, entity.SeasonNumber)
                                        .Set(e => e.EpisodeNumber, entity.EpisodeNumber).Set(e => e.Name, entity.Name)
                                        .Set(e => e.Description, entity.Description).Set(e => e.Poster, entity.Poster)
                                        .Set(e => e.GuestActors, entity.GuestActors).Set(e => e.Directors, entity.Directors);

            collection.FindAndModify(query, SortBy.Null, update);
        }

        /// <summary>
        /// Delete one episode.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// Needs to delete also the object synopse in season episodes list.
        protected override void HookDelete(Tuple<string, int, int> id)
        {
            var collection = this.Database.GetCollection(id.Item1);
            var query = Query.And(Query<Episode>.EQ(e => e.TvShowId, id.Item1), Query<Episode>.EQ(e => e.SeasonNumber, id.Item2), Query<Episode>.EQ(e => e.EpisodeNumber, id.Item3));

            var synopsis = this.Read(id).GetSynopsis();

            // In this case first remove the object synopse than remove the episode, because can not have
            // one synopse for one episode that not exists.
            this.seasonsRepository.RemoveEpisodeSynopsis(id.Item1, id.Item2, synopsis);
            collection.FindAndRemove(query, SortBy.Null);
        }

        /// <summary>
        /// The try add to newest document.
        /// </summary>
        /// <param name="episode">
        /// The episode.
        /// </param>
        private void TryAddToNewestDocument(Episode episode)
        {
            try
            {
                if (episode.Date == null || (DateTime.Parse(episode.Date) < DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd"))))
                {
                    return;
                }

                var newwestDoc = this.newestCollection.FindOneByIdAs<NewestEpisodes>(episode.TvShowId);
                if (newwestDoc == null)
                {
                    newwestDoc = new NewestEpisodes(episode.TvShowId);
                    this.newestCollection.Insert(newwestDoc);
                }
            }
            catch (Exception)
            {
                // TODO, add to log mechanism.
                return;
            }

            var query = Query<NewestEpisodes>.EQ(we => we.Key, episode.TvShowId);
            var update = Update<NewestEpisodes>.Push(we => we.Episodes, episode.GetSynopsis());
            this.ModifyList(this.newestCollection, query, update);
        }
    }
}