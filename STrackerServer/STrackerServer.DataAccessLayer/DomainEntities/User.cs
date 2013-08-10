// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of user domain entity. The Id is the email of the user.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The user.
    /// </summary>
    public class User : Person, IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {    
            this.Friends = new List<UserSynopsis>();
            this.FriendRequests = new List<UserSynopsis>();
            this.Subscriptions = new List<Subscription>();
            this.Suggestions = new List<Suggestion>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public User(string id) : this()
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the version of the entity.
        /// </summary>
        public int Version { get; set; }

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
        public List<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Gets or sets the suggestions.
        /// </summary>
        public List<Suggestion> Suggestions { get; set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        public int Permission { get; set; }

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
            // Compare name and photo for verify if is necessary to update the information.
            return Name.Equals(other.Name) && Photo.Equals(other.Photo);
        }

        /// <summary>
        /// The get synopsis.
        /// </summary>
        /// <returns>
        /// The <see cref="UserSynopsis"/>.
        /// </returns>
        public UserSynopsis GetSynopsis()
        {
            var uri = string.Format("users/{0}", this.Id);
            return new UserSynopsis { Id = this.Id, Name = this.Name, Photo = this.Photo, Uri = uri };
        }

        /// <summary>
        /// The user synopsis.
        /// </summary>
        public class UserSynopsis : ISynopsis
        {
            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the photo.
            /// </summary>
            public string Photo { get; set; }

            /// <summary>
            /// Gets or sets the uri.
            /// </summary>
            public string Uri { get; set; }
        }
    }
}