// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RatingsBase.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of base ratings domain entity. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.Ratings
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
    public class RatingsBase<T> : IEntity<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RatingsBase{T}"/> class.
        /// </summary>
        public RatingsBase()
        {
            this.Ratings = new List<Rating>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingsBase{T}"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public RatingsBase(T id) : this()
        {
            this.Key = id;
        } 

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public T Key { get; set; }

        /// <summary>
        /// Gets or sets the average.
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        /// Gets or sets the ratings.
        /// </summary>
        public List<Rating> Ratings { get; set; }
    }
}