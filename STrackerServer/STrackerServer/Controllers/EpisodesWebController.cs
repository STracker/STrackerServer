// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesWebController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the EpisodesWebController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using STrackerServer.BusinessLayer.Core;
using STrackerServer.DataAccessLayer.DomainEntities;
using STrackerServer.Models.Episode;

namespace STrackerServer.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// The episodes web controller.
    /// </summary>
    public class EpisodesWebController : BaseWebController
    {
        /// <summary>
        /// The season operations.
        /// </summary>
        private readonly ITvShowsOperations showOps;

        /// <summary>
        /// The episodes operations.
        /// </summary>
        private readonly IEpisodesOperations episodesOps;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesWebController"/> class.
        /// </summary>
        /// <param name="showOps">
        /// The season ops.
        /// </param>
        /// <param name="episodesOps">
        /// The episodes ops.
        /// </param>
        public EpisodesWebController(ITvShowsOperations showOps, IEpisodesOperations episodesOps)
        {
            this.showOps = showOps;
            this.episodesOps = episodesOps;
        }


        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Show(string tvshowId, int seasonNumber, int number)
        {
            OperationResultState state;
            Episode episode = this.episodesOps.TryRead(new Tuple<string, int, int>(tvshowId, seasonNumber, number), out state);

            if (episode == null)
            {
                return this.GetView(state);
            }

            EpisodeView model = new EpisodeView
            {
                TvShowId = episode.Key.Item1,
                SeasonNumber = episode.Key.Item2,
                EpisodeNumber = episode.Key.Item3,
                Description = episode.Description,
                Name = episode.Name,
                Rating = episode.Rating,
            };

            return this.View(model);
        }

    }
}
