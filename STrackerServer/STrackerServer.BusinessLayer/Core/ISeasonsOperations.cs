// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISeasonsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over seasons.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.BusinessLayer.Core
{
    using System;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Seasons operations interface.
    /// </summary>
    public interface ISeasonsOperations : ICrudOperations<Season, Tuple<string, int>>
    {
        // Addictional actions...
    }
}