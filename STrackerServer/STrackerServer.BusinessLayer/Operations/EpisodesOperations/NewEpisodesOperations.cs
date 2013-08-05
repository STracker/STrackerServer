// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewEpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The new episodes operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.EpisodesOperations
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The new episodes operations.
    /// </summary>
    public class NewEpisodesOperations : INewEpisodesOperations
    {
        /// <summary>
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// The newest episodes repository.
        /// </summary>
        private readonly INewestEpisodesRepository newestEpisodesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewEpisodesOperations"/> class.
        /// </summary>
        /// <param name="tvshowsOperations">
        /// The television shows operations.
        /// </param>
        /// <param name="newestEpisodesRepository">
        /// The newest Episodes Repository.
        /// </param>
        public NewEpisodesOperations(ITvShowsOperations tvshowsOperations, INewestEpisodesRepository newestEpisodesRepository)
        {
            this.tvshowsOperations = tvshowsOperations;
            this.newestEpisodesRepository = newestEpisodesRepository;
        }

        /// <summary>
        /// The get newest episodes.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public ICollection<Episode.EpisodeSynopsis> GetNewEpisodes(string tvshowId, string date)
        {
            // Verify date format.
            DateTime temp;
            if (date != null && !DateTime.TryParse(date, out temp))
            {
                return null;
            }

            var tvshow = this.tvshowsOperations.Read(tvshowId);
            if (tvshow == null)
            {
                return null;
            }

            var newTvShowEpisodes = this.newestEpisodesRepository.Read(tvshowId);
            if (newTvShowEpisodes == null)
            {
                return null;
            }

            return date == null ? newTvShowEpisodes.Episodes : newTvShowEpisodes.Episodes.Where(epi => DateTime.Parse(epi.Date) <= DateTime.Parse(date)).ToList();
        }

        /// <summary>
        /// The get newest episodes.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public ICollection<NewTvShowEpisodes> GetNewEpisodes(string date = null)
        {
            DateTime temp;

            if (date != null && !DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
            {
                return null;
            }

            var newEpisodes = this.newestEpisodesRepository.GetAll();
            var retList = new List<NewTvShowEpisodes>();
            if (date != null)
            {
                foreach (var tvshow in newEpisodes)
                {
                    tvshow.Episodes = tvshow.Episodes.Where(epi => DateTime.Parse(epi.Date) <= DateTime.Parse(date)).ToList();

                    if (tvshow.Episodes.Count > 0)
                    {
                        retList.Add(tvshow);
                    }
                }
            }

            return retList;
        }
    }
}
