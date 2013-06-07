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
    using System;
    using System.Configuration;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    using STrackerUpdater.RabbitMQ;
    using STrackerUpdater.RabbitMQ.Core;

    /// <summary>
    /// The episodes comments operations.
    /// </summary>
    public class EpisodesCommentsOperations : BaseCommentsOperations<Episode, CommentsEpisode, Tuple<string, int, int>>, IEpisodesCommentsOperations 
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
        /// The remove comment hook.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool RemoveCommentHook(Tuple<string, int, int> key, Comment comment)
        {
            return this.CommentsRepository.RemoveComment(key, comment);
        }

        /// <summary>
        /// The add comment hook method.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        protected override void AddCommentHook(Tuple<string, int, int> key, Comment comment)
        {
            this.QueueM.Push(
               new Message
               {
                   CommandName = ConfigurationManager.AppSettings["EpisodeCommentCmd"],
                   Arg = string.Format("{0}|{1}|{2}|{3}|{4}", key.Item1, key.Item2, key.Item3, comment.UserId, comment.Body)
               });
        }
    }
}