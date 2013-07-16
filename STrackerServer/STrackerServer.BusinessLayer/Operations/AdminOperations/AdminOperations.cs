// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdminOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the AdminOperations type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.AdminOperations
{
    using STrackerServer.BusinessLayer.Core.AdminOperations;
    using STrackerServer.BusinessLayer.Permissions;
    using STrackerServer.DataAccessLayer.Core.UsersRepositories;

    /// <summary>
    /// The admin operations.
    /// </summary>
    public class AdminOperations : IAdminOperations
    {
        /// <summary>
        /// The users repository.
        /// </summary>
        private readonly IUsersRepository usersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminOperations"/> class.
        /// </summary>
        /// <param name="usersRepository">
        /// The users repository.
        /// </param>
        public AdminOperations(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        /// <summary>
        /// The set user permission.
        /// </summary>
        /// <param name="adminId">
        /// The admin id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="permission">
        /// The permission.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SetUserPermission(string adminId, string userId, Permission permission)
        {
            var admin = this.usersRepository.Read(adminId);
            var user = this.usersRepository.Read(userId);

            if (admin == null || user == null)
            {
                return false;
            }

            if (admin.Permission != (int)Permission.Admin || (user.Permission == (int)Permission.Admin && !adminId.Equals(userId)))
            {
                return false;
            }

            return this.usersRepository.SetUserPermission(user, (int)permission);
        }
    }
}
