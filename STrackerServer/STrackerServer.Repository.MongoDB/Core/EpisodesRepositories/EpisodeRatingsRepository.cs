// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episodes ratings repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.EpisodesRepositories
{
    using System.Linq;

    using global::MongoDB.Bson;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The episode ratings repository.
    /// </summary>
    public class EpisodeRatingsRepository : BaseRatingsRepository<RatingsEpisode, Episode.EpisodeKey>, IEpisodeRatingsRepository 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeRatingsRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public EpisodeRatingsRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
        }

        /// <summary>
        /// The add rating.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool AddRating(Episode.EpisodeKey id, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvshowId, CollectionPrefix));
            var query = Query<RatingsEpisode>.EQ(r => r.Id, id);
            var update = Update<RatingsEpisode>.Push(er => er.Ratings, rating);

            var removeRating = this.Read(id).Ratings.Find(r => r.User.Id.Equals(rating.User.Id));

            // If already have a rating for the user, need to remove it before insert the new one.
            if (this.RemoveRating(id, removeRating) && this.ModifyList(collection, query, update, this.Read(id)))
            {
                // Update average rating.
                return this.Update(this.Read(id));
            }

            return false;
        }

        /// <summary>
        /// The remove rating.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool RemoveRating(Episode.EpisodeKey id, Rating rating)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvshowId, CollectionPrefix));
            var query = Query<RatingsEpisode>.EQ(r => r.Id, id);
            var update = Update<RatingsEpisode>.Pull(er => er.Ratings, rating);
            return this.ModifyList(collection, query, update, this.Read(id));
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(RatingsEpisode entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.Id.TvshowId, CollectionPrefix));
            collection.Insert(entity);
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="RatingsEpisode"/>.
        /// </returns>
        protected override RatingsEpisode HookRead(Episode.EpisodeKey id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvshowId, CollectionPrefix));
            return collection.FindOneByIdAs<RatingsEpisode>(id.ToBsonDocument());
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(RatingsEpisode entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.Id, CollectionPrefix));
            var update = Update<RatingsEpisode>.Set(e => e.Average, entity.Ratings.Average(rating => rating.UserRating)).Set(e => e.Version, entity.Version + 1);
            var query = Query<RatingsEpisode>.EQ(ratings => ratings.Id, entity.Id);

            collection.FindAndModify(query, SortBy.Null, update);
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(Episode.EpisodeKey id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvshowId, CollectionPrefix));
            var query = Query<RatingsEpisode>.EQ(r => r.Id, id);
            collection.FindAndRemove(query, SortBy.Null);
        }
    }
}
