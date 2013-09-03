// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ISeasonsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.SeasonsOperations
{
    using System.Collections.Generic;
    using System.Linq;

    using STrackerServer.BusinessLayer.Core.SeasonsOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Seasons operations.
    /// </summary>
    public class SeasonsOperations : BaseCrudOperations<ISeasonsRepository, Season, Season.SeasonId>, ISeasonsOperations
    {
        /// <summary>
        /// Television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="tvshowsOperations">
        /// Television shows operations.
        /// </param>
        public SeasonsOperations(ISeasonsRepository repository, ITvShowsOperations tvshowsOperations)
            : base(repository)
        {
            this.tvshowsOperations = tvshowsOperations;
        }

        /// <summary>
        ///  Get one season from one television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Season"/>.
        /// </returns>
        public override Season Read(Season.SeasonId id)
        {
            var tvshow = this.tvshowsOperations.Read(id.TvShowId);

            if (tvshow == null)
            {
                return null;
            }

            var season = this.Repository.Read(id);

            if (season == null)
            {
                return null;
            }

            season.Episodes = season.Episodes.OrderBy(synopsis => synopsis.Id.EpisodeNumber).ToList();

            return season;
        }

        /// <summary>
        /// Get all seasons synopsis from one television show.
        /// </summary>
        /// <param name="tvshowId">
        /// Television show id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public ICollection<Season.SeasonSynopsis> GetAllFromOneTvShow(string tvshowId)
        {
            var tvshow = this.tvshowsOperations.Read(tvshowId);
            return tvshow == null ? null : tvshow.Seasons;
        }
    }
}