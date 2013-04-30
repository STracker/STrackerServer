// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System.Collections.Generic;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Television shows operations.
    /// </summary>
    public class TvShowsOperations : BaseCrudOperations<TvShow, string>, ITvShowsOperations
    {
        /// <summary>
        /// The works repository.
        /// </summary>
        private readonly ICreateTvShowWorksRepository worksRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="worksRepository">
        ///  Work queue for create television shows information.
        /// </param>
        public TvShowsOperations(ITvShowsRepository repository, ICreateTvShowWorksRepository worksRepository)
            : base(repository)
        {
            this.worksRepository = worksRepository;
        }

        /// <summary>
        /// Try get one television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="resultState">
        /// The resultState.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public TvShow TryRead(string id, out OperationResultState resultState)
        {
            // Verifiy if exists one work for this operation.
            var work = this.worksRepository.Read(id);
            if (work != null)
            {
                // Work already exists, so try get the information later.
                resultState = OperationResultState.InProcess;
                return null;
            }

            // Verify if exists in database.
            var tvshow = this.Repository.Read(id);
            if (tvshow != null)
            {
                resultState = OperationResultState.Completed;
                return tvshow;
            }

            // If don't exists the work and don't exists the information in database, creates one work.
            try
            {
                if (!this.worksRepository.Create(new CreateTvShowWork { Key = id }))
                {
                    resultState = OperationResultState.Error;
                    return null;
                }

                resultState = OperationResultState.InProcess;
            }
            catch (DuplicatedIdException)
            {
                resultState = OperationResultState.InProcess;          
            }
            catch (InvalidIdException)
            {
                resultState = OperationResultState.NotFound;
            }

            return null;
        }

        /// <summary>
        /// Get television shows by genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<TvShow> ReadAllByGenre(Genre genre)
        {
            return ((ITvShowsRepository)this.Repository).ReadAllByGenre(genre);
        }

        /// <summary>
        /// Try get one television show by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public TvShow TryReadByName(string name, out OperationResultState state)
        {
            var id = this.worksRepository.GetId(name);

            if (id == null)
            {
                state = OperationResultState.NotFound;
                return null;
            }

            return this.TryRead(id, out state);
        }
    }
}