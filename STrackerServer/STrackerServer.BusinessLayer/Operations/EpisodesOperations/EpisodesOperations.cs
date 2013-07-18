// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.EpisodesOperations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.SeasonsOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes operations.
    /// </summary>
    public class EpisodesOperations : BaseCrudOperations<Episode, Tuple<string, int, int>>, IEpisodesOperations
    {
        /// <summary>
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// The seasons operations.
        /// </summary>
        private readonly ISeasonsOperations seasonsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesOperations"/> class.
        /// </summary>
        /// <param name="tvshowsOperations">
        /// The television shows Operations.
        /// </param>
        /// <param name="seasonsOperations">
        /// The seasons operations.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public EpisodesOperations(ITvShowsOperations tvshowsOperations, ISeasonsOperations seasonsOperations, IEpisodesRepository repository)
            : base(repository)
        {
            this.tvshowsOperations = tvshowsOperations;
            this.seasonsOperations = seasonsOperations;
        }

        /// <summary>
        /// Get one episode from one season from one television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="STrackerServer.DataAccessLayer.DomainEntities.Episode"/>.
        /// </returns>
        public override Episode Read(Tuple<string, int, int> id)
        {
            var season = this.seasonsOperations.Read(new Tuple<string, int>(id.Item1, id.Item2));
            return (season == null) ? null : this.Repository.Read(id);
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
            var season = this.seasonsOperations.Read(new Tuple<string, int>(tvshowId, seasonNumber));
            return season == null ? null : ((IEpisodesRepository)this.Repository).GetAllFromOneSeason(tvshowId, seasonNumber);
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
        public IEnumerable<Episode.EpisodeSynopsis> GetNewestEpisodes(string tvshowId, string date)
        {
            // Verify date format.
            DateTime temp;
            if (!DateTime.TryParse(date, out temp))
            {
                return null;
            }

            var tvshow = this.tvshowsOperations.Read(tvshowId);
            if (tvshow == null)
            {
                return null;
            }

            var episodes = (List<Episode.EpisodeSynopsis>)((IEpisodesRepository)this.Repository).GetNewestEpisodes(tvshowId);

            return date == null ? episodes : episodes.Where(epi => DateTime.Parse(epi.Date) <= DateTime.Parse(date));
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
        public IEnumerable<Episode.EpisodeSynopsis> GetNewestEpisodes(string date)
        {
            DateTime temp;
            if (!DateTime.TryParse(date, out temp))
            {
                return null;
            }

            var episodes = ((IEpisodesRepository)this.Repository).GetNewestEpisodes();
            return date == null ? episodes : episodes.Where(epi => DateTime.Parse(epi.Date) <= DateTime.Parse(date));
        }
    }
}