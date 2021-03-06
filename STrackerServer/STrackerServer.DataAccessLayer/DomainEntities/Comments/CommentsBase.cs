﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentsBase.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of base comments domain entity. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.Comments
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The comments.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the key.
    /// </typeparam>
    public class CommentsBase<T> : IEntity<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsBase{T}"/> class.
        /// </summary>
        public CommentsBase()
        {
            this.Comments = new List<Comment>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsBase{T}"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public CommentsBase(T id) : this()
        {
            this.Id = id;
        } 

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        /// Gets or sets the version of the entity.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public List<Comment> Comments { get; set; }
    }
}