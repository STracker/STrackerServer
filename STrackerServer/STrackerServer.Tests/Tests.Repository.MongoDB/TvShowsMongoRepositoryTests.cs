// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsMongoRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Unit Tests for mongo television shows repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Tests.Repository.MongoDB
{
    using global::MongoDB.Driver;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Tests.NinjectModules;

    /// <summary>
    /// Television shows mongo repository tests.
    /// </summary>
    [TestFixture]
    public class TvShowsMongoRepositoryTests
    {
        /// <summary>
        /// Television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsMongoRepositoryTests"/> class.
        /// </summary>
        public TvShowsMongoRepositoryTests()
        {
            using (IKernel kernel = new StandardKernel(new TestModuleForMongoRepositories()))
            {
                this.tvshowsRepository = kernel.Get<ITvShowsRepository>();
            }
        }

        /// <summary>
        /// Create  operations test.
        /// </summary>
        /// "tt0436992-test" is a id for one television show created specifically for testing. 
        [Test(Description = "Create operation test for television shows mongo repository.")]
        public void Create()
        {
            /*
             * Delete
             
            Assert.IsTrue(this.tvshowsRepository.Delete("tt0436992-test")); - Can not make the Delete because the delete of television shows
                                                                                                                       drop the collection romoving also the season and episode created for testing.

            /*
             * Create
             */
            var tvshow = new TvShow("tt0436992-test")
            {
                Name = "Doctor Who",
                Description = "The continuing adventures of The Doctor, an alien time traveler - a Time Lord - from Gallifrey. Together with his companions they travel through time and space in the TARDIS, battling evil where they find it.",
                Rating = 4,
                Runtime = 50
            };

            try
            {
                Assert.IsTrue(this.tvshowsRepository.Create(tvshow));
            }
            catch (WriteConcernException)
            {
                // Allready exists
                Assert.IsTrue(true);
            }
        }

        /// <summary>
        /// Create fail operation test.
        /// </summary>
        [Test(Description = "Create operation test for television shows mongo repository. Fails because the try to create one television show with one id that already exists."), ExpectedException(typeof(WriteConcernException))]
        public void CreateFail()
        {
            var tvshow = new TvShow("tt0436992-test");

            this.tvshowsRepository.Create(tvshow);
        }

        /// <summary>
        /// Read operation test.
        /// </summary>
        [Test(Description = "Read operation test for television shows mongo repository.")]
        public void Read()
        {
            var tvshow = this.tvshowsRepository.Read("tt0436992-test");

            Assert.NotNull(tvshow);

            Assert.AreNotEqual(tvshow.Id, "tt123456");

            Assert.AreEqual(tvshow.Id, "tt0436992-test");
            Assert.AreEqual(tvshow.Name, "Doctor Who");
            Assert.AreEqual(tvshow.Description, "The continuing adventures of The Doctor, an alien time traveler - a Time Lord - from Gallifrey. Together with his companions they travel through time and space in the TARDIS, battling evil where they find it.");
            Assert.AreEqual(tvshow.Rating, 4);
            Assert.AreEqual(tvshow.Runtime, 50);
        }

        /// <summary>
        /// Update operation test.
        /// </summary>
        [Test(Description = "Update operation test for television shows mongo repository.")]
        public void Update()
        {
            var tvshow = this.tvshowsRepository.Read("tt0436992-test");

            Assert.NotNull(tvshow);
            Assert.AreEqual(tvshow.Name, "Doctor Who");
            Assert.AreEqual(tvshow.Rating, 4);

            // Update rating to 5.
            tvshow.Rating = 5;

            Assert.IsTrue(this.tvshowsRepository.Update(tvshow));

            tvshow = this.tvshowsRepository.Read("tt0436992-test");

            Assert.AreNotEqual(tvshow.Rating, 4);
            Assert.AreEqual(tvshow.Rating, 5);

            // Update rating for old value, 4.
            tvshow.Rating = 4;

            Assert.IsTrue(this.tvshowsRepository.Update(tvshow));

            tvshow = this.tvshowsRepository.Read("tt0436992-test");

            Assert.AreEqual(tvshow.Rating, 4);
        }
    }
}
