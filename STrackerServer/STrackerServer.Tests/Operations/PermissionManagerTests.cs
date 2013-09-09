// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PermissionManagerTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The permission manager tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Operations
{
    using Ninject;

    using NUnit.Framework;

    using STrackerServer.BusinessLayer.Permissions;

    /// <summary>
    /// The permission manager tests.
    /// </summary>
    [TestFixture]
    public class PermissionManagerTests
    {
        /// <summary>
        /// The permission manager.
        /// </summary>
        private readonly IPermissionManager<Permissions, int> permissionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionManagerTests"/> class.
        /// </summary>
        public PermissionManagerTests()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.permissionManager = kernel.Get<IPermissionManager<Permissions, int>>();
        }

        /// <summary>
        /// Test get permission.
        /// </summary>
        [Test]
        public void GetPermission()
        {
            Assert.AreEqual(Permissions.Default, this.permissionManager.GetPermission(0));
            Assert.AreEqual(Permissions.Moderator, this.permissionManager.GetPermission(1));
            Assert.AreEqual(Permissions.Admin, this.permissionManager.GetPermission(2));
        }

        /// <summary>
        /// Test has permission.
        /// </summary>
        [Test]
        public void HasPermission()
        {
            Assert.True(this.permissionManager.HasPermission(Permissions.Default, (int)Permissions.Admin));
            Assert.True(this.permissionManager.HasPermission(Permissions.Default, (int)Permissions.Moderator));
            Assert.True(this.permissionManager.HasPermission(Permissions.Default, (int)Permissions.Default));

            Assert.False(this.permissionManager.HasPermission(Permissions.Admin, (int)Permissions.Moderator));
            Assert.False(this.permissionManager.HasPermission(Permissions.Moderator, (int)Permissions.Default));
        }
    }
}
