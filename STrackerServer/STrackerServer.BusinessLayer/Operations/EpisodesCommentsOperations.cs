// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesCommentsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesCommnentsOperations interface.
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
    /// The episodes comments operations.
    /// </summary>
    public class EpisodesCommentsOperations : BaseCommentsOperations<Episode, EpisodeComments, Tuple<string, int, int>>, IEpisodesCommentsOperations 
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
        /// The remove comment.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="userId">
        /// The user Id.
        /// </param>
        /// <param name="commentPosition">
        /// The comment Position.
        /// </param>
        public override void RemoveComment(Tuple<string, int, int> key, string userId, int commentPosition)
        {
            this.QueueM.Push(new Message
                {
                    CommandName = "episodeCommentRemove",
                    Arg = string.Format("{0}|{1}|{2}|{3}|{4}", key.Item1, key.Item2, key.Item3, userId, commentPosition)
                });
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
                   CommandName = "episodeCommentAdd",
                   Arg = string.Format("{0}|{1}|{2}|{3}|{4}", key.Item1, key.Item2, key.Item3, comment.UserId, comment.Body)
               });
        }
    }
}