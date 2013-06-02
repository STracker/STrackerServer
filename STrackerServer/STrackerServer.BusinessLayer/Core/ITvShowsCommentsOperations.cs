// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowsCommentsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over television shows comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The television shows comments operations interface.
    /// </summary>
    public interface ITvShowsCommentsOperations : ICommentsOperations<TvShowComments, string>
    {
    }
}