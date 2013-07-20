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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using STrackerBackgroundWorker.RabbitMQ;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Permissions;
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
    public abstract class BaseCommentsOperations<T, TC, TK> : ICommentsOperations<TC, TK> where T : IEntity<TK> where TC : CommentsBase<TK> 
    {
        /// <summary>
        /// The comments repository.
        /// </summary>
        protected readonly ICommentsRepository<TC, TK> CommentsRepository;

        /// <summary>
        /// The entity repository.
        /// </summary>
        protected readonly IRepository<T, TK> Repository;

        /// <summary>
        /// The queue manager.
        /// </summary>
        protected readonly QueueManager QueueM;

        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// The permission manager.
        /// </summary>
        private readonly IPermissionManager<Permission, int> permissionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommentsOperations{T,TC,TK}"/> class.
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
        /// <param name="usersOperations">
        /// The users operations.
        /// </param>
        /// <param name="permissionManager">
        /// The permission manager.
        /// </param>
        protected BaseCommentsOperations(ICommentsRepository<TC, TK> commentsRepository, IRepository<T, TK> repository, QueueManager queueM, IUsersOperations usersOperations, IPermissionManager<Permission, int> permissionManager)
        {
            this.CommentsRepository = commentsRepository;
            this.Repository = repository;
            this.QueueM = queueM;
            this.usersOperations = usersOperations;
            this.permissionManager = permissionManager;
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
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
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
        /// The get comments.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="TC"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public TC GetComments(TK key)
        {
            var entity = this.Repository.Read(key);
            return Equals(entity, null) ? default(TC) : this.CommentsRepository.Read(key);
        }

        /// <summary>
        /// The remove comment.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="userId">
        /// The user Key.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public bool RemoveComment(TK key, string userId, string id)
        {
            var entity = this.Repository.Read(key);
            if (Equals(entity, default(T)))
            {
                return false;
            }

            var user = this.usersOperations.Read(userId);

            Comment comment;

            if (this.permissionManager.HasPermission(Permission.Moderator, user.Permission))
            {
                comment = this.CommentsRepository.Read(key).Comments.FirstOrDefault(c => c.Id.Equals(id));
            }
            else
            {
                comment = this.CommentsRepository.Read(key).Comments.FirstOrDefault(c => c.User.Id.Equals(userId) && c.Id.Equals(id));
            }

            return comment != null && this.RemoveCommentHook(key, comment);
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
        protected abstract bool RemoveCommentHook(TK key, Comment comment);

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
