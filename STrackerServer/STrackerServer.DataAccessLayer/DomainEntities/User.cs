// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of user domain entity. The Key is the email of the user.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System.Collections.Generic;

    /// <summary>
    /// The user.
    /// </summary>
    public class User : Person, IComparable<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {    
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        public User(string email) : base(email)
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
            return new UserSynopsis { Email = this.Key, Name = this.Name };
        }

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="other">
        /// Other User.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CompareTo(User other)
        {
            return (Name.Equals(other.Name) && Photo.ImageUrl.Equals(other.Photo.ImageUrl)) ? 1 : 0;
        }

        /// <summary>
        /// The user synopsis.
        /// </summary>
        public class UserSynopsis
        {
            /// <summary>
            /// Gets or sets the id. The id is the email.
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }
        }
    }
}
