// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoriesSetup.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The test setup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories
{
    using NUnit.Framework;

    /// <summary>
    /// The test setup.
    /// </summary>
    // [SetUpFixture]
    public class RepositoriesSetup
    {
        /// <summary>
        /// The setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            Utils.CleanDatabase();
        }

        /// <summary>
        /// The tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Utils.CleanDatabase();
        }
    }
}
