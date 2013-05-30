// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Container.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The container.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// The container.
    /// </summary>
    /// <typeparam name="T">
    ///  Key Type
    /// </typeparam>
    /// <typeparam name="TK">
    ///  Items Type
    /// </typeparam>
    public abstract class Container<T, TK> : IEntity<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Container{T,TK}"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="container">
        /// The container.
        /// </param>
        protected Container(T key, string container)
        {
            this.Key = key;
            this.ContainerType = container;
            this.Items = new List<TK>();
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public T Key { get; set; }

        /// <summary>
        /// Gets or sets the container type.
        /// </summary>
        public string ContainerType { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public List<TK> Items { get; set; }
    }
}
