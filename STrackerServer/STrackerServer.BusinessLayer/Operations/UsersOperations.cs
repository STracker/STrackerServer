// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The users operations.
    /// </summary>
    public class UsersOperations : BaseCrudOperations<User, string>, IUsersOperations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public UsersOperations(IUsersRepository repository) : base(repository)
        {
        }

        /// <summary>
<<<<<<< HEAD
        /// Create one user.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(User entity)
        {
            return Repository.Create(entity);
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        public override User Read(string id)
        {
            return Repository.Read(id);
        }

        public override User ReadAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Update one user.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(User entity)
        {
            return Repository.Update(entity);
        }

        /// <summary>
=======
>>>>>>> c40d384839a81f8e0f9eb2b3ef8286b77ce5f5bb
        /// Verify if the user exists, if not create one. Also verify if the properties of the user have changed.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool VerifyAndSave(User user)
        {
            var domainUser = this.Repository.Read(user.Key);
            if (domainUser == null)
            {
                return this.Create(user);
            }

            if (user.CompareTo(domainUser) == 0)
            {
                return true;
            }

            // For not lose the friends its necessary to "pass" them to new object user for update.
            user.Friends = domainUser.Friends;
            return this.Update(user);
        }
    }
}
