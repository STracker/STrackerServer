// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTvShowWork.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of create television show work domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// Work for create television show information.
    /// </summary>
    public class CreateTvShowWork : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get;  set; }
    }
}
