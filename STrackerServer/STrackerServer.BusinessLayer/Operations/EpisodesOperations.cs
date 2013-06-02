// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    using STrackerUpdater.RabbitMQ;
    using STrackerUpdater.RabbitMQ.Core;

    /// <summary>
    /// Episodes operations.
    /// </summary>
    public class EpisodesOperations : BaseCrudOperations<Episode, Tuple<string, int, int>>, IEpisodesOperations
    {
        /// <summary>
        /// The seasons operations.
        /// </summary>
        private readonly ISeasonsOperations seasonsOperations;

        /// <summary>
        /// The comments repository.
        /// </summary>
        private readonly IEpisodeCommentsRepository commentsRepository;

        /// <summary>
        /// The queue manager.
        /// </summary>
        private readonly QueueManager queueM;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesOperations"/> class.
        /// </summary>
        /// <param name="seasonsOperations">
        /// The seasons operations.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="commentsRepository">
        /// The comments Repository.
        /// </param>
        /// <param name="queueM">
        /// The queue m.
        /// </param>
        public EpisodesOperations(ISeasonsOperations seasonsOperations, IEpisodesRepository repository, IEpisodeCommentsRepository commentsRepository, QueueManager queueM)
            : base(repository)
        {
            this.seasonsOperations = seasonsOperations;
            this.commentsRepository = commentsRepository;
            this.queueM = queueM;
        }

        /// <summary>
        /// Get one episode from one season from one television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Episode"/>.
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
        /// The add comment.
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
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddComment(string tvshowId, int seasonNumber, int episodeNumber, Comment comment)
        {
            var episode = this.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));

            if (episode == null)
            {
                return false;
            }

            this.queueM.Push(
                new Message
                {
                    CommandName = "episodeComment",
                    Arg = string.Format("{0}|{1}|{2}|{3}|{4}", tvshowId, seasonNumber, episodeNumber, comment.UserId, comment.Body)
                });

            return true;
        }

        /// <summary>
        /// The get comments.
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
        /// The <see cref="EpisodeComments"/>.
        /// </returns>
        public EpisodeComments GetComments(string tvshowId, int seasonNumber, int episodeNumber)
        {
            var episode = this.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));

            return episode == null ? null : this.commentsRepository.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));
        }
    }
}