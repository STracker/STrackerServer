// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAsyncOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of async operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// The AsyncOperations interface.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity's Key.
    /// </typeparam>
    public interface IAsyncOperations<T, in TK> : ICrudOperations<T, TK> where T : IEntity<TK>
    {
        /// <summary>
        /// Try read the information. If the information don't exists is created one work for get the information.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T TryRead(TK id, out OperationResultState state);
    }
}