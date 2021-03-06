﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestsSetup.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The tests setup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// The tests setup.
    /// </summary>
    [SetUpFixture]
    public class TestsSetup
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
