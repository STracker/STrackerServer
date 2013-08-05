// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenresOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The genre operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System.Collections.Generic;
    using System.Linq;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The genre operations.
    /// </summary>
    public class GenresOperations : BaseCrudOperations<Genre, string>, IGenresOperations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenresOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public GenresOperations(IGenresRepository repository)
            : base(repository)
        {
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Genre"/>.
        /// </returns>
        public override Genre Read(string id)
        {
            return id == null ? null : this.Repository.Read(id);
        }

        /// <summary>
        /// Get all genres.
        /// </summary>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public ICollection<Genre.GenreSynopsis> GetAll()
        {
            return ((IGenresRepository)this.Repository).GetAll();
        }

        /// <summary>
        /// The get television shows.
        /// </summary>
        /// <param name="genres">
        /// The genres.
        /// </param>
        /// <param name="maxtvShows">
        /// The max television shows.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public ICollection<TvShow.TvShowSynopsis> GetTvShows(ICollection<string> genres, int maxtvShows)
        {
            var retList = new List<TvShow.TvShowSynopsis>();

            if (maxtvShows == 0)
            {
                return retList;
            }

            var results = new Dictionary<string, TvShowGenreResult>();

            foreach (var genre in genres.Select(this.Read).Where(genre => genre != null))
            {
                foreach (var tvshow in genre.TvshowsSynopsis)
                {
                    TvShowGenreResult result;

                    if (results.ContainsKey(tvshow.Id))
                    {
                        result = results[tvshow.Id];
                    }
                    else
                    {
                        result = new TvShowGenreResult { TvShow = tvshow, TotalGenres = genres.Count };
                        results.Add(tvshow.Id, result);
                    }

                    result.GenreCount++;
                }
            }

            var orderedResults = results.OrderByDescending(pair => pair.Value.Percentage);

            var count = 0;
            foreach (var result in orderedResults)
            {
                if (count++ == maxtvShows)
                {
                    break;
                }

                retList.Add(result.Value.TvShow);
            }

            return retList;
        }

        /// <summary>
        /// The television show genre result.
        /// </summary>
        private class TvShowGenreResult
        {
            /// <summary>
            /// Gets or sets the television show.
            /// </summary>
            public TvShow.TvShowSynopsis TvShow { get; set; }

            /// <summary>
            /// Gets or sets the total genres.
            /// </summary>
            public int TotalGenres { private get; set; }

            /// <summary>
            /// Gets or sets the genre count.
            /// </summary>
            public int GenreCount { get; set; }

            /// <summary>
            /// Gets the percentage.
            /// </summary>
            public double Percentage
            {
                get
                {
                    return this.GenreCount / (double)this.TotalGenres;
                }
            }
        }
    }
}
