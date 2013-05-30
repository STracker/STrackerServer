// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The comments repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The comments repository.
    /// </summary>
    public class CommentsRepository : ICommentsRepository
    {
        /// <summary>
        /// MongoDB database object.
        /// </summary>
        protected readonly MongoDatabase Database;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        protected CommentsRepository(MongoClient client, MongoUrl url)
        {
            this.Database = client.GetServer().GetDatabase(url.DatabaseName);
        }

        /// <summary>
        /// The create television show comments.
        /// </summary>
        /// <param name="comments">
        /// The comments.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CreateTvShowComments(TvShowComments comments)
        {
            return this.Database.GetCollection(comments.Key).Insert(comments).Ok;
        }

        /// <summary>
        /// The create episode comments.
        /// </summary>
        /// <param name="comments">
        /// The comments.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CreateEpisodeComments(EpisodeComments comments)
        {
            return this.Database.GetCollection(comments.Key.Item1).Insert(comments).Ok;
        }

        /// <summary>
        /// The read television show comments.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public TvShowComments ReadTvShowComments(string tvshowId)
        {
            var query = Query.And(
                Query<TvShowComments>.EQ(comments => comments.Key, tvshowId),
                Query<TvShowComments>.EQ(comments => comments.ContainerType, "Comments"));
            return this.Database.GetCollection(tvshowId).FindOneAs<TvShowComments>(query);
        }

        /// <summary>
        /// The read episode comments.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonId">
        /// The season id.
        /// </param>
        /// <param name="episodeNumber">
        /// The episode number.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public EpisodeComments ReadEpisodeComments(string tvshowId, string seasonId, int episodeNumber)
        {
            var query = Query.And(
                Query<EpisodeComments>.EQ(comments => comments.Key.Item1, tvshowId),
                Query<EpisodeComments>.EQ(comments => comments.Key.Item2, seasonId),
                Query<EpisodeComments>.EQ(comments => comments.Key.Item3, episodeNumber),
                Query<EpisodeComments>.EQ(comments => comments.ContainerType, "Comments"));

            return this.Database.GetCollection(tvshowId).FindOneAs<EpisodeComments>(query);
        }

        /// <summary>
        /// The update show comment.
        /// </summary>
        /// <param name="comments">
        /// The comments.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UpdateShowComment(TvShowComments comments)
        {
            var query = Query.And(
                Query<TvShowComments>.EQ(c => c.Key, comments.Key),
                Query<TvShowComments>.EQ(c => c.ContainerType, "Comments"));

            var update = Update<TvShowComments>.Set(showComments => showComments.Items, comments.Items);
            return this.Database.GetCollection(comments.Key).Update(query, update).Ok;
        }

        /// <summary>
        /// The update episode comment.
        /// </summary>
        /// <param name="comments">
        /// The comments.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UpdateEpisodeComment(EpisodeComments comments)
        {
            var query = Query.And(
                Query<EpisodeComments>.EQ(c => c.Key.Item1, comments.Key.Item1),
                Query<EpisodeComments>.EQ(c => c.Key.Item2, comments.Key.Item2),
                Query<EpisodeComments>.EQ(c => c.Key.Item3, comments.Key.Item3),
                Query<EpisodeComments>.EQ(c => c.ContainerType, "Comments"));

            var update = Update<EpisodeComments>.Set(showComments => showComments.Items, comments.Items);

            return this.Database.GetCollection(comments.Key.Item1).Update(query, update).Ok;
        }
    }
}
