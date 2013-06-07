// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITvShowCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The Television Show comments Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core.TvShowsRepositories
{
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    /// <summary>
    /// The Television Show Comments Repository interface.
    /// </summary>
    public interface ITvShowCommentsRepository : ICommentsRepository<CommentsTvShow, string>
    {
    }
}