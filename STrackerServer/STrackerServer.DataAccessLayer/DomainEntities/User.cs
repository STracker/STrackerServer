// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of user domain entity. The Key is the email of the user.
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
        public User()
        {    
            this.Friends = new List<UserSynopsis>();
            this.FriendRequests = new List<UserSynopsis>();
            this.SubscriptionList = new List<TvShow.TvShowSynopsis>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public User(string id)
            : base(id)
        {
            this.Friends = new List<UserSynopsis>();
            this.FriendRequests = new List<UserSynopsis>();
            this.SubscriptionList = new List<TvShow.TvShowSynopsis>();
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the friends.
        /// </summary>
        public List<UserSynopsis> Friends { get; set; }

        /// <summary>
        /// Gets or sets the friend requests.
        /// </summary>
        public List<UserSynopsis> FriendRequests { get; set; } 

        /// <summary>
        /// Gets or sets the subscription list.
        /// </summary>
        public List<TvShow.TvShowSynopsis> SubscriptionList { get; set; }

        /// <summary>
        /// The get synopsis.
        /// </summary>
        /// <returns>
        /// The <see cref="UserSynopsis"/>.
        /// </returns>
        public UserSynopsis GetSynopsis()
        {
            return new UserSynopsis { Id = this.Key };
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
        public bool Equals(User other)
        {
            return Name.Equals(other.Name) && Photo.ImageUrl.Equals(other.Photo.ImageUrl);
        }

        /// <summary>
        /// The user synopsis.
        /// </summary>
        public class UserSynopsis
        {
            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            public string Id { get; set; }
        }
    }
}
