﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utils.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The tests utils.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using MongoDB.Driver;

    using Ninject;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The test utils.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// The default poster.
        /// </summary>
        private const string DefaultPoster = "https://dl.dropboxusercontent.com/u/2696848/image-not-found.gif";

        /// <summary>
        /// The test database name.
        /// </summary>
        private static readonly string DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];

        /// <summary>
        /// The database.
        /// </summary>
        private static readonly MongoDatabase Database;

        /// <summary>
        /// Initializes static members of the <see cref="Utils"/> class.
        /// </summary>
        static Utils()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            Database = kernel.Get<MongoClient>().GetServer().GetDatabase(DatabaseName);
        }

        /// <summary>
        /// Clean database.
        /// </summary>
        public static void CleanDatabase()
        {
            foreach (var collectionName in Database.GetCollectionNames().Where(name => !name.Equals("system.indexes") && !name.Equals("system.users")))
            {
                Database.DropCollection(collectionName);
            }
        }

        private static int idCounter;

        /// <summary>
        /// The create id.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>
        /// </returns>
        public static string CreateId()
        {
            return Interlocked.Increment(ref idCounter).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The get dummy television show modal.
        /// </summary>
        /// <param name="id">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public static TvShow CreateTvShow(string id)
        {
            var tvshow = new TvShow(id)
            {
                Name = "Name",
                Poster = DefaultPoster,
                Description = "Description",
                AirDay = "AirDay",
                AirTime = "AirTime",
                FirstAired = "FirstAired"
            };
            tvshow.Seasons.Add(CreateSeason(id, 1).GetSynopsis());
            tvshow.Seasons.Add(CreateSeason(id, 2).GetSynopsis());

            tvshow.Genres.Add(CreateGenre("Genre1").GetSynopsis());
            tvshow.Genres.Add(CreateGenre("Genre2").GetSynopsis());

            return tvshow;
        }

        /// <summary>
        /// The create season dummy.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <returns>
        /// The <see cref="Season"/>.
        /// </returns>
        public static Season CreateSeason(string tvshowId, int seasonNumber)
        {
            var season = new Season(new Season.SeasonId { TvShowId = tvshowId, SeasonNumber = seasonNumber });
            season.Episodes.Add(new Episode.EpisodeSynopsis
            {
                Id = new Episode.EpisodeId { TvShowId = tvshowId, SeasonNumber = seasonNumber, EpisodeNumber = seasonNumber },
                Name = "Name",
                Date = "2020-01-01",
                Uri = string.Format("tvshows/{0}/seasons/{1}/episodes/{2}", tvshowId, seasonNumber, 1)
            });
            
            return season;
        }

        /// <summary>
        /// The create episode dummy.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="episodeNumber">
        /// The episode number.
        /// </param>
        /// <returns>
        /// The <see cref="Episode"/>.
        /// </returns>
        public static Episode CreateEpisode(string tvshowId, int seasonNumber, int episodeNumber)
        {
            var episode = new Episode(new Episode.EpisodeId { TvShowId = tvshowId, SeasonNumber = seasonNumber, EpisodeNumber = episodeNumber })
            {
                Name = "Name",
                Description = "Description",
                Poster = DefaultPoster,
                Date = "2020-01-01"
            };

            return episode;
        }

        /// <summary>
        /// The create genre.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="Genre"/>.
        /// </returns>
        public static Genre CreateGenre(string name)
        {
            var genre = new Genre(name);
            return genre;
        }

        /// <summary>
        /// The create user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        public static User CreateUser(string id)
        {
            var user = new User(id) { Name = "Name", Photo = "Photo", Email = "Email", };
            return user;
        }
    }
}
