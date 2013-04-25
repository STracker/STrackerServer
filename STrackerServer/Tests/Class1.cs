using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    using MongoDB.Driver;

    using NUnit.Framework;

    using STrackerServer.BusinessLayer.Operations;
    using STrackerServer.Repository.MongoDB.Core;
    using STrackerServer.WorkQueue;

    [TestFixture]
    public class Class1
    {
        [Test]
        public void Test1()
        {
            var work = new WorkQueueForTvShows();


            oper.Read("tt1520211");
        }
    }
}
