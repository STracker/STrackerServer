// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesCommentsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesCommnentsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.EpisodesOperations
{
    using System.Configuration;

    using STrackerBackgroundWorker.RabbitMQ;
    using STrackerBackgroundWorker.RabbitMQ.Core;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    /// <summary>
    /// The episodes comments operations.
    /// </summary>
    public class EpisodesCommentsOperations : BaseCommentsOperations<Episode, CommentsEpisode, Episode.EpisodeId>, IEpisodesCommentsOperations 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesCommentsOperations"/> class.
        /// </summary>
        /// <param name="commentsRepository">
        /// The comments repository.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="queueM">
        /// The queue m.
        /// </param>
        public EpisodesCommentsOperations(IEpisodeCommentsRepository commentsRepository, IEpisodesRepository repository, QueueManager queueM)
            : base(commentsRepository, repository, queueM)
        {
        }

        /// <summary>
        /// The add comment hook method.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        protected override void AddCommentHook(Episode.EpisodeId id, Comment comment)
        {
            this.QueueM.Push(
               new Message
               {
                   CommandName = ConfigurationManager.AppSettings["EpisodeCommentCmd"],
                   Arg = string.Format("{0}|{1}|{2}|{3}|{4}", id.TvShowId, id.SeasonNumber, id.EpisodeNumber, comment.User.Id, comment.Body)
               });
        }
    }
}