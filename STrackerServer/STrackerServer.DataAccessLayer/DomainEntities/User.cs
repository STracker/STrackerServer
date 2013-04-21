// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of user domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System.Collections.Generic;

    /// <summary>
    /// The user.
    /// </summary>
    public class User : Person
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public User(string key) : base(key)
        {
        }

        /// <summary>
        /// Gets or sets the friends.
        /// </summary>
        public List<UserSynopsis> Friends { get; set; }

        /// <summary>
        /// The get synopsis.
        /// </summary>
        /// <returns>
        /// The <see cref="UserSynopsis"/>.
        /// </returns>
        public UserSynopsis GetSynopsis()
        {
            return new UserSynopsis { Id = this.Id, Name = this.Name };
        }

        /// <summary>
        /// The user synopsis.
        /// </summary>
        public class UserSynopsis
        {
            /// <summary>
            /// Gets or sets the id. The id is the email.
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }
        }
    }
}
