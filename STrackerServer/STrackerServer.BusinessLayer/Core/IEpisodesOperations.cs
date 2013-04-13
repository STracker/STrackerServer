// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over episodes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using System;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes operations interface.
    /// </summary>
    public interface IEpisodesOperations : ICrudOperations<Episode, Tuple<string, int, int>>
    {
        // Addictional actions...
    }
}
