// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ICommentsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    using STrackerUpdater.RabbitMQ;
    using STrackerUpdater.RabbitMQ.Core;

    /// <summary>
    /// The comments operations.
    /// </summary>
    public class CommentsOperations : ICommentsOperations
    {
        /// <summary>
        /// The television shows comments repository.
        /// </summary>
        private readonly ITvShowCommentsRepository commentsTvshowsRepository;

        /// <summary>
        /// The episodes comments repository.
        /// </summary>
        private readonly IEpisodeCommentsRepository commentsEpisodesRepository;

        /// <summary>
        /// The television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// The episodes repository.
        /// </summary>
        private readonly IEpisodesRepository episodesRepository;

        /// <summary>
        /// The queue manager.
        /// </summary>
        private readonly QueueManager queueM;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsOperations"/> class.
        /// </summary>
        /// <param name="commentsTvshowsRepository">
        /// The television shows comments Repository.
        /// </param>
        /// <param name="commentsEpisodesRepository">
        /// The comments Episodes Repository.
        /// </param>
        /// <param name="tvshowsRepository">
        /// The television shows repository.
        /// </param>
        /// <param name="episodesRepository">
        /// The episodes repository.
        /// </param>
        /// <param name="queueM">
        /// The queue m.
        /// </param>
        public CommentsOperations(ITvShowCommentsRepository commentsTvshowsRepository, IEpisodeCommentsRepository commentsEpisodesRepository, ITvShowsRepository tvshowsRepository, IEpisodesRepository episodesRepository, QueueManager queueM)
        {
            this.commentsTvshowsRepository = commentsTvshowsRepository;
            this.commentsEpisodesRepository = commentsEpisodesRepository;
            this.tvshowsRepository = tvshowsRepository;
            this.episodesRepository = episodesRepository;
            this.queueM = queueM;
        }

        /// <summary>
        /// The add comment.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddTvShowComment(string tvshowId, Comment comment)
        {
            var tvshow = this.tvshowsRepository.Read(tvshowId);

            if (tvshow == null)
            {
                return false;
            }

            this.queueM.Push(
                new Message
                {
                    CommandName = "tvShowComment",
                    Arg = string.Format("{0}|{1}|{2}", tvshowId, comment.UserId, comment.Body)
                });

            return true;
        }

        /// <summary>
        /// The get comments.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShowComments"/>.
        /// </returns>
        public TvShowComments GetTvShowComments(string tvshowId)
        {
            var tvshow = this.tvshowsRepository.Read(tvshowId);

            return tvshow == null ? null : this.commentsTvshowsRepository.Read(tvshowId);
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
        public bool AddEpisodeComment(string tvshowId, int seasonNumber, int episodeNumber, Comment comment)
        {
            var episode = this.episodesRepository.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber));

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
        public EpisodeComments GetEpisodeComments(string tvshowId, int seasonNumber, int episodeNumber)
        {
            var key = new Tuple<string, int, int>(tvshowId, seasonNumber, episodeNumber);
            var episode = this.episodesRepository.Read(key);

            return episode == null ? null : this.commentsEpisodesRepository.Read(key);
        }
    }
}
