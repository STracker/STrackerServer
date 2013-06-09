// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesWebController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the EpisodesWebController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Action_Results;
    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.Models.Episode;

    /// <summary>
    /// The episodes web controller.
    /// </summary>
    public class EpisodesWebController : Controller
    {
        /// <summary>
        /// The episodes operations.
        /// </summary>
        private readonly IEpisodesOperations episodesOps;

        /// <summary>
        /// The television shows ops.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOps;

        /// <summary>
        /// The comments operations.
        /// </summary>
        private readonly IEpisodesCommentsOperations commentsOperations;

        /// <summary>
        /// The ratings operations.
        /// </summary>
        private readonly IEpisodesRatingsOperations ratingsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesWebController"/> class.
        /// </summary>
        /// <param name="episodesOps">
        /// The episodes ops.
        /// </param>
        /// <param name="tvshowsOps">
        /// The television shows ops.
        /// </param>
        /// <param name="commentsOperations">
        /// The comments Operations.
        /// </param>
        /// <param name="ratingsOperations">
        /// The ratings Operations.
        /// </param>
        public EpisodesWebController(IEpisodesOperations episodesOps, ITvShowsOperations tvshowsOps, IEpisodesCommentsOperations commentsOperations, IEpisodesRatingsOperations ratingsOperations)
        {
            this.episodesOps = episodesOps;
            this.tvshowsOps = tvshowsOps;
            this.commentsOperations = commentsOperations;
            this.ratingsOperations = ratingsOperations;
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
        /// <param name="episodeNumber">
        /// The episode Number.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Show(string tvshowId, int seasonNumber, int episodeNumber)
        {
            var episode = this.episodesOps.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));

            if (episode == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var tvshow = this.tvshowsOps.Read(tvshowId);

            var model = new EpisodeView
            {
                TvShowId = episode.TvShowId,
                SeasonNumber = episode.SeasonNumber,
                EpisodeNumber = episode.EpisodeNumber,
                Description = episode.Description,
                Name = episode.Name,
                GuestActors = episode.GuestActors,
                Directors = episode.Directors,
                Poster = tvshow.Poster.ImageUrl,
                TvShowName = tvshow.Name,
            };

            return this.View(model);
        }

        /// <summary>
        /// The comments.
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
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Comments(string tvshowId, int seasonNumber, int episodeNumber)
        {
            var episodeComments = this.commentsOperations.GetComments(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));
            
            if (episodeComments == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }  

            var view = new EpisodeComments
                {
                    TvShowId = tvshowId,
                    SeasonNumber = seasonNumber,
                    EpisodeNumber = episodeNumber,
                    Comments = episodeComments.Comments,
                    Poster = this.tvshowsOps.Read(tvshowId).Poster.ImageUrl
                };

            return this.View(view);
        }

        /// <summary>
        /// The create comment.
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
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult CreateComment(string tvshowId, int seasonNumber, int episodeNumber)
        {
            var episode = this.episodesOps.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));

            if (episode == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var view = new EpisodeCreateComment
                {
                    TvShowId = tvshowId,
                    SeasonNumber = seasonNumber,
                    EpisodeNumber = episodeNumber,
                    Poster = this.tvshowsOps.Read(tvshowId).Poster.ImageUrl
                };

            return this.View(view);
        }

        /// <summary>
        /// The create comment.
        /// </summary>
        /// <param name="create">
        /// The create.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult CreateComment(EpisodeCreateComment create)
        {
            var episode = this.episodesOps.Read(new Tuple<string, int, int>(create.TvShowId, create.SeasonNumber, create.EpisodeNumber));

            if (episode == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            var tvshow = this.tvshowsOps.Read(create.TvShowId);

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                create.Poster = tvshow.Poster.ImageUrl;
                return this.View(create);
            }

            var comment = new Comment { Body = create.Body, UserId = User.Identity.Name, Timestamp = Environment.TickCount.ToString(CultureInfo.InvariantCulture) };

            this.commentsOperations.AddComment(new Tuple<string, int, int>(create.TvShowId, create.SeasonNumber, create.EpisodeNumber), comment);
            return new SeeOtherResult { Url = Url.Action("Comments", "EpisodesWeb", new { tvshowId = create.TvShowId, seasonNumber = create.SeasonNumber, episodeNumber = create.SeasonNumber }) };
        }
    }
}