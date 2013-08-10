// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCommentsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ICommentsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using STrackerBackgroundWorker.RabbitMQ;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    /// <summary>
    /// The comments operations.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the entity.
    /// </typeparam>
    /// <typeparam name="TC">
    /// Type of the comment entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of the key.
    /// </typeparam>
    public abstract class BaseCommentsOperations<T, TC, TK> : BaseCrudOperations<ICommentsRepository<TC, TK>, TC, TK>, ICommentsOperations<TC, TK> where T : IEntity<TK> where TC : CommentsBase<TK> 
    {
        /// <summary>
        /// The entity repository.
        /// </summary>
        protected readonly IRepository<T, TK> EntityRepository;

        /// <summary>
        /// The queue manager.
        /// </summary>
        protected readonly QueueManager QueueM;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommentsOperations{T,TC,TK}"/> class.
        /// </summary>
        /// <param name="commentsRepository">
        /// The comments repository.
        /// </param>
        /// <param name="entityRepository">
        /// The entity Repository.
        /// </param>
        /// <param name="queueM">
        /// The queue m.
        /// </param>
        protected BaseCommentsOperations(ICommentsRepository<TC, TK> commentsRepository, IRepository<T, TK> entityRepository, QueueManager queueM)
            : base(commentsRepository)
        {
            this.EntityRepository = entityRepository;
            this.QueueM = queueM;
        }

        /// <summary>
        /// The add comment.
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
        public bool AddComment(TK key, Comment comment)
        {
            var entity = this.Repository.Read(key);
            if (Equals(entity, default(T)))
            {
                return false;
            }

            this.AddCommentHook(key, comment);
            return true;
        }

        /// <summary>
        /// The remove comment.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="userId">
        /// The user Id.
        /// </param>
        /// <param name="commentId">
        /// The comment Id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveComment(TK id, string userId, string commentId)
        {
            var entity = this.EntityRepository.Read(id);
            if (Equals(entity, default(T)))
            {
                return false;
            }

            var comment = this.Repository.Read(id).Comments.Find(c => c.Id.Equals(commentId));

            return comment != null && this.Repository.RemoveComment(id, comment);
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TC"/>.
        /// </returns>
        public override TC Read(TK id)
        {
            return this.Repository.Read(id);
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
        protected abstract void AddCommentHook(TK key, Comment comment);
    }
}
