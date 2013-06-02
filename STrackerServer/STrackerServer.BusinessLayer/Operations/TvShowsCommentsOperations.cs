// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsCommentsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsCommentsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    using STrackerUpdater.RabbitMQ;
    using STrackerUpdater.RabbitMQ.Core;

    /// <summary>
    /// The television shows comments operations.
    /// </summary>
    public class TvShowsCommentsOperations : CommentsOperations<TvShow, TvShowComments, string>, ITvShowsCommentsOperations 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsCommentsOperations"/> class.
        /// </summary>
        /// <param name="commentsRepository">
        /// The comments Repository.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="queueM">
        /// The queue m.
        /// </param>
        public TvShowsCommentsOperations(ITvShowCommentsRepository commentsRepository, ITvShowsRepository repository, QueueManager queueM)
            : base(commentsRepository, repository, queueM)
        {
        }

        /// <summary>
        /// The remove comment.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        public override void RemoveComment(string key, Comment comment)
        {
            this.QueueM.Push(new Message
                {
                    CommandName = "tvShowCommentRemove",
                    Arg = string.Format("{0}|{1}|{2}", key, comment.UserId, comment.Index)
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
        protected override void AddCommentHook(string key, Comment comment)
        {
            this.QueueM.Push(
                new Message
                {
                    CommandName = "tvShowCommentAdd",
                    Arg = string.Format("{0}|{1}|{2}", key, comment.UserId, comment.Body)
                });
        }
    }
}
