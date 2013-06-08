// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsCommentsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsCommentsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.TvShowsOperations
{
    using System.Configuration;

    using STrackerBackgroundWorker.RabbitMQ;
    using STrackerBackgroundWorker.RabbitMQ.Core;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    /// <summary>
    /// The television shows comments operations.
    /// </summary>
    public class TvShowsCommentsOperations : BaseCommentsOperations<TvShow, CommentsTvShow, string>, ITvShowsCommentsOperations 
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
        protected override void AddCommentHook(string key, Comment comment)
        {
            this.QueueM.Push(
                new Message
                {
                    CommandName = ConfigurationManager.AppSettings["TvShowCommentCmd"],
                    Arg = string.Format("{0}|{1}|{2}|{3}", key, comment.UserId, comment.Timestamp, comment.Body)
                });
        }
    }
}