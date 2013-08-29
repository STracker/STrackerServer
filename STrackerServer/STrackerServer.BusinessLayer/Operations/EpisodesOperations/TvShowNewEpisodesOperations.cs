// --------------------------------------------------------------------------------------------------------------------
// <copyright file="tvShowNewEpisodesOperations.cs" company="STracker">
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
    using System.Linq;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The new episodes operations.
    /// </summary>
    public class TvShowNewEpisodesOperations : BaseCrudOperations<ITvShowNewEpisodesRepository, NewTvShowEpisodes, string>, ITvShowNewEpisodesOperations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowNewEpisodesOperations"/> class.
        /// </summary>
        /// <param name="newEpisodesRepository">
        /// The new Episodes Repository.
        /// </param>
        public TvShowNewEpisodesOperations(ITvShowNewEpisodesRepository newEpisodesRepository)
            : base(newEpisodesRepository)
        {
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="NewTvShowEpisodes"/>.
        /// </returns>
        public override NewTvShowEpisodes Read(string id)
        {
            return this.Repository.Read(id);
        }

        /// <summary>
        /// Get the new episodes from one television show up to the date in parameters.
        /// If the date is null, return all new episodes from the television show.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="NewTvShowEpisodes"/>.
        /// </returns>
        public NewTvShowEpisodes GetNewEpisodes(string tvshowId, DateTime? date)
        {
            var ne = this.Repository.Read(tvshowId);
            ne.Episodes = ne.Episodes.OrderBy(synopsis => synopsis.Date).ToList();

            if (date == null)
            {
                return ne;
            }

            ne.Episodes.RemoveAll(e => DateTime.Parse(e.Date) > date || DateTime.Parse(e.Date) < DateTime.UtcNow.Date);
            return ne;
        }

        /// <summary>
        /// Get new episodes from all television shows.
        /// If the date is null, return all new episodes from all television shows.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<NewTvShowEpisodes> GetNewEpisodes(DateTime? date)
        {
            var all = this.Repository.ReadAll();

            if (date == null)
            {
                return all;
            }

            var allEpis = new List<NewTvShowEpisodes>();
            foreach (var entry in all)
            {
                var ne = entry;
                ne.Episodes.RemoveAll(e => DateTime.Parse(e.Date) > date || DateTime.Parse(e.Date) < DateTime.UtcNow.Date);
                if (ne.Episodes.Count > 0)
                {
                    ne.Episodes = ne.Episodes.OrderBy(synopsis => synopsis.Date).ToList();
                    allEpis.Add(ne);
                }
            }

            return allEpis;
        }
    }
}
