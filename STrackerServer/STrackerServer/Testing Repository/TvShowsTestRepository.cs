// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsTestRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Testing_Repository
{
    using System;
    using System.Collections.Generic;
    using STrackerServer.Models.Media;
    using STrackerServer.Models.Person;
    using STrackerServer.Models.Utils;

    /// <summary>
    /// The television shows test repository.
    /// </summary>
    public class TvShowsTestRepository
    {
        /// <summary>
        /// The dictionary.
        /// </summary>
        private readonly IDictionary<string, TvShow> dictionary = new Dictionary<string, TvShow>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsTestRepository"/> class.
        /// </summary>
        public TvShowsTestRepository()
        {
            var season1 = new Season
                {
                    Number = 1
                };

            var season2 = new Season
            {
                Number = 2
            };

            var tvshow = new TvShow
                {
                    Id = "tt1520211",
                    Title = "The Walking Dead",
                    Description = "Police officer Rick Grimes leads a group of survivors in a world overrun by zombies.",
                    Runtime = 45,
                    Rating = 4,
                    FirstAired = new DateTime(2010, 10, 31),
                    Creator = new Person { Name = "Frank Darabont" },
                    Genres = new List<Genre> { Genre.Drama, Genre.Horror, Genre.Thriller },
                    Seasons = new List<Season> { season1, season2 }
                };

            this.dictionary.Add(tvshow.Id, tvshow);
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="show">
        /// The television show.
        /// </param>
        public void Add(TvShow show)
        {
            this.dictionary.Add(show.Id, show);
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public TvShow Get(string id)
        {
            TvShow tvshow;
            this.dictionary.TryGetValue(id, out tvshow);
            return tvshow;
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Contains(string id)
        {
            return dictionary.ContainsKey(id);
        }
    }
}