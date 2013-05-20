// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Unit tests for television shows operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Operations.Tests
{
    using Ninject;

    using NUnit.Framework;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.NinjectDependencies;

    /// <summary>
    /// The television shows operations.
    /// </summary>
    [TestFixture]
    public class TvShowsOperations
    {
        /// <summary>
        /// The operations.
        /// </summary>
        private readonly ITvShowsOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsOperations"/> class.
        /// </summary>
        public TvShowsOperations()
        {
            using (IKernel kernel = new StandardKernel(new ModuleForSTracker()))
            {
                this.operations = kernel.Get<ITvShowsOperations>();
            }
        }

        /// <summary>
        /// The read by IMDB id test.
        /// </summary>
        [Test]
        public void Read()
        {
            var tvshow = this.operations.Read("tt1520211");

            Assert.AreEqual(tvshow.TvShowId, "tt1520211");
            Assert.AreEqual(tvshow.Name, "The Walking Dead");
            Assert.AreEqual(tvshow.Description, "Based on the comic book series of the same name, The Walking Dead tells the story of a small group of survivors living in the aftermath of a zombie apocalypse.");
            Assert.AreEqual(tvshow.FirstAired, "2010-10-31");
            Assert.AreEqual(tvshow.AirDay, "Sunday");
            Assert.AreEqual(tvshow.Runtime, 45);
            Assert.AreEqual(tvshow.Actors.Count, 19);
            Assert.AreEqual(tvshow.SeasonSynopses.Count, 5);
        }
    }
}