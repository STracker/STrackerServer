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
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using STrackerServer.Action_Results;
    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
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
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

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
        /// The comments operations.
        /// </param>
        /// <param name="ratingsOperations">
        /// The ratings operations.
        /// </param>
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        public EpisodesWebController(IEpisodesOperations episodesOps, ITvShowsOperations tvshowsOps, IEpisodesCommentsOperations commentsOperations, IEpisodesRatingsOperations ratingsOperations, IUsersOperations usersOperations)
        {
            this.episodesOps = episodesOps;
            this.tvshowsOps = tvshowsOps;
            this.commentsOperations = commentsOperations;
            this.ratingsOperations = ratingsOperations;
            this.usersOperations = usersOperations;
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
            var key = new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber);

            var episode = this.episodesOps.Read(key);

            if (episode == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var tvshow = this.tvshowsOps.Read(tvshowId);

            var isSubscribed = false;
            var watched = false;

            if (User.Identity.IsAuthenticated)
            {
                var user = this.usersOperations.Read(User.Identity.Name);
                var subscription = user.SubscriptionList.Find(subscription1 => subscription1.TvShow.Id.Equals(tvshowId));

                if (subscription != null)
                {
                    isSubscribed = true;
                    watched = subscription.EpisodesWatched.Exists(synopsis => synopsis.Equals(episode.GetSynopsis()));
                }
            }

            var episodeDate = DateTime.ParseExact(episode.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var systemDate = DateTime.Now;

            bool asAired = !(DateTime.Compare(episodeDate, systemDate) > 0);

            var model = new EpisodeView
            {
                TvShowId = episode.TvShowId,
                SeasonNumber = episode.SeasonNumber,
                EpisodeNumber = episode.EpisodeNumber,
                Description = episode.Description,
                Name = episode.Name,
                GuestActors = episode.GuestActors,
                Directors = episode.Directors,
                Poster = episode.Poster ?? tvshow.Poster,
                TvShowName = tvshow.Name,
                Date = episode.Date,
                Rating = this.ratingsOperations.Read(key).Average,
                IsSubscribed = isSubscribed,
                Watched = watched,
                AsAired = asAired
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

            var episode = this.episodesOps.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));
            var tvshow = this.tvshowsOps.Read(tvshowId);

            var view = new EpisodeComments
                {
                    TvShowId = tvshowId,
                    SeasonNumber = seasonNumber,
                    EpisodeNumber = episodeNumber,
                    Comments = episodeComments.Comments,
                    Poster = episode.Poster ?? tvshow.Poster,
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

            var tvshow = this.tvshowsOps.Read(tvshowId);

            var view = new EpisodeCreateComment
                {
                    TvShowId = tvshowId,
                    SeasonNumber = seasonNumber,
                    EpisodeNumber = episodeNumber,
                    Poster = episode.Poster ?? tvshow.Poster,
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
                create.Poster = episode.Poster ?? tvshow.Poster;
                return this.View(create);
            }

            var comment = new Comment { Body = create.Body, User = this.usersOperations.Read(User.Identity.Name).GetSynopsis() };

            this.commentsOperations.AddComment(new Tuple<string, int, int>(create.TvShowId, create.SeasonNumber, create.EpisodeNumber), comment);
            return new SeeOtherResult { Url = Url.Action("Comments", "EpisodesWeb", new { tvshowId = create.TvShowId, seasonNumber = create.SeasonNumber, episodeNumber = create.EpisodeNumber }) };
        }

        /// <summary>
        /// The comment.
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
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Comment(string tvshowId, int seasonNumber, int episodeNumber, string id)
        {
            var comments =
                this.commentsOperations.GetComments(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));

            if (comments == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var comment = comments.Comments.FirstOrDefault(comment1 => comment1.Id.Equals(id));

            if (comment == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            if (!comment.User.Id.Equals(User.Identity.Name))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.View("Error", Response.StatusCode);
            }

            var tvshow = this.tvshowsOps.Read(tvshowId);
            var episode = this.episodesOps.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));

            var commentView = new EpisodeComment
            {
                TvShowId = tvshowId,
                SeasonNumber = seasonNumber,
                EpisodeNumber = episodeNumber,
                UserId = comment.User.Id,
                Body = comment.Body,
                Id = comment.Id,
                Poster = episode.Poster ?? tvshow.Poster
            };

            return this.View(commentView);
        }

        /// <summary>
        /// The remove comment.
        /// </summary>
        /// <param name="remove">
        /// The remove.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult RemoveComment(EpisodeRemoveComment remove)
        {
            if (!ModelState.IsValid || !this.commentsOperations.RemoveComment(new Tuple<string, int, int>(remove.TvShowId, remove.SeasonNumber, remove.EpisodeNumber), User.Identity.Name, remove.Id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Comments", "EpisodesWeb", new { tvshowId = remove.TvShowId, seasonNumber = remove.SeasonNumber, episodeNumber = remove.EpisodeNumber }) };
        }

        /// <summary>
        /// The rate.
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
        public ActionResult Rate(string tvshowId, int seasonNumber, int episodeNumber)
        {
            var episode = this.episodesOps.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));

            if (episode == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            return this.View(new EpisodeRating
            {
                TvShowId = tvshowId,
                SeasonNumber = seasonNumber,
                EpisodeNumber = episodeNumber,
                Poster = episode.Poster ?? this.tvshowsOps.Read(tvshowId).Poster,
                Value = 1
            });
        }

        /// <summary>
        /// The rate.
        /// </summary>
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Rate(EpisodeRating rating)
        {
            var episode = this.episodesOps.Read(new Tuple<string, int, int>(rating.TvShowId, rating.SeasonNumber, rating.EpisodeNumber));

            if (episode == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            var tvshow = this.tvshowsOps.Read(rating.TvShowId);

            if (!ModelState.IsValid)
            {
                rating.Poster = episode.Poster ?? tvshow.Poster;
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View(rating);
            }

            if (!this.ratingsOperations.AddRating(new Tuple<string, int, int>(rating.TvShowId, rating.SeasonNumber, rating.EpisodeNumber), new Rating { UserId = User.Identity.Name, UserRating = rating.Value }))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult { Url = Url.Action("Show", new { tvshowId = rating.TvShowId, seasonNumber = rating.SeasonNumber, episodeNumber = rating.EpisodeNumber }) };
        }

        /// <summary>
        /// The watched.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Watched(EpisodeWatched values)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            bool success;

            if (values.Watched)
            {
                success = this.usersOperations.AddWatchedEpisode(User.Identity.Name, values.TvShowId, values.SeasonNumber, values.EpisodeNumber);
            }
            else
            {
                success = this.usersOperations.RemoveWatchedEpisode(User.Identity.Name, values.TvShowId, values.SeasonNumber, values.EpisodeNumber);
            }

            if (!success)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            return new SeeOtherResult
                {
                    Url = Url.Action(
                        "Show",
                        "EpisodesWeb",
                        new
                            {
                                tvshowId = values.TvShowId,
                                seasonNumber = values.SeasonNumber,
                                episodeNumber = values.EpisodeNumber
                            })
                };
        }
    }
}