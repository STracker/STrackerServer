// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseComments.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of base comments domain entity. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// The comments.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the key.
    /// </typeparam>
    public class BaseComments<T> : IEntity<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseComments{T}"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public BaseComments(T key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public T Key { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public List<Comment> Comments { get; set; } 
    }
}