// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewEpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The new episodes operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.EpisodesOperations
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The new episodes operations.
    /// </summary>
    public class NewEpisodesOperations : INewEpisodesOperations
    {
        /// <summary>
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// The episodes repository.
        /// </summary>
        private readonly IEpisodesRepository episodesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewEpisodesOperations"/> class.
        /// </summary>
        /// <param name="tvshowsOperations">
        /// The television shows operations.
        /// </param>
        /// <param name="usersOperations">
        /// The users Operations.
        /// </param>
        /// <param name="episodesRepository">
        /// The episodes Repository.
        /// </param>
        public NewEpisodesOperations(ITvShowsOperations tvshowsOperations, IUsersOperations usersOperations, IEpisodesRepository episodesRepository)
        {
            this.tvshowsOperations = tvshowsOperations;
            this.usersOperations = usersOperations;
            this.episodesRepository = episodesRepository;
        }

        /// <summary>
        /// The get newest episodes.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public ICollection<Episode.EpisodeSynopsis> GetNewEpisodes(string tvshowId, string date)
        {
            // Verify date format.
            DateTime temp;
            if (date != null && !DateTime.TryParse(date, out temp))
            {
                return null;
            }

            var tvshow = this.tvshowsOperations.Read(tvshowId);
            if (tvshow == null)
            {
                return null;
            }

            var newTvShowEpisodes = this.episodesRepository.GetNewestEpisodes(tvshowId);

            if (newTvShowEpisodes == null)
            {
                return null;
            }

            return date == null ? newTvShowEpisodes.Episodes : newTvShowEpisodes.Episodes.Where(epi => DateTime.Parse(epi.Date) <= DateTime.Parse(date)).ToList();
        }

        /// <summary>
        /// The get newest episodes.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public ICollection<NewTvShowEpisodes> GetNewEpisodes(string date = null)
        {
            DateTime temp;

            if (date != null && !DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
            {
                return null;
            }

            var newEpisodes = this.episodesRepository.GetNewestEpisodes();

            var retList = new List<NewTvShowEpisodes>();

            if (date != null)
            {
                foreach (var tvshow in newEpisodes)
                {
                    tvshow.Episodes = tvshow.Episodes.Where(epi => DateTime.Parse(epi.Date) <= DateTime.Parse(date)).ToList();

                    if (tvshow.Episodes.Count > 0)
                    {
                        retList.Add(tvshow);
                    }
                }
            }

            return retList;
        }

        /// <summary>
        /// The get newest episodes.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public ICollection<NewTvShowEpisodes> GetUserNewEpisodes(string userId)
        {
            var user = this.usersOperations.Read(userId);

            if (user == null)
            {
                return null;
            }

            var retList = new List<NewTvShowEpisodes>();

            foreach (var subscription in user.SubscriptionList)
            {
                var episodes = this.GetNewEpisodes(subscription.TvShow.Id, DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"));
                
                if (episodes == null)
                {
                   continue; 
                }
                
                var episodesList = episodes.ToList();

                if (episodesList.Count != 0)
                {
                    retList.Add(new NewTvShowEpisodes(subscription.TvShow.Id)
                    {
                        TvShow = subscription.TvShow,
                        Episodes = episodesList
                    });
                }
            }

            return retList;
        }
    }
}
