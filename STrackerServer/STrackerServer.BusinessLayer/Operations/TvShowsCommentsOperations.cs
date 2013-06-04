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
    public class TvShowsCommentsOperations : BaseCommentsOperations<TvShow, TvShowComments, string>, ITvShowsCommentsOperations 
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
        protected override bool RemoveCommentHook(string key, Comment comment)
        {
            return ((ITvShowCommentsRepository)this.CommentsRepository).RemoveComment(key, comment);
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
                    Arg = string.Format("{0}|{1}|{2}|{3}", key, comment.UserId, comment.Timestamp, comment.Body)
                });
        }
    }
}
