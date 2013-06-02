// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEpisodesCommentsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over episodes comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using System;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The episodes comments operations interface.
    /// </summary>
    public interface IEpisodesCommentsOperations : ICommentsOperations<EpisodeComments, Tuple<string, int, int>>
    {
    }
}
