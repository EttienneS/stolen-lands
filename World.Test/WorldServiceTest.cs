using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace World.Test
{
    [TestClass]
    public class WorldServiceTest
    {

        [TestMethod]
        public void WorldServiceTest_ctor()
        {
            var width = Helper.Random.Next(1, 10);
            var height = Helper.Random.Next(1, 10);

            var w = new WorldService(width, height);
            Assert.IsTrue(w.Locations != null);
            Assert.IsTrue(w.Width == width);
            Assert.IsTrue(w.Height == height);
        }

        [TestMethod]
        public void Generate_World()
        {
            var width = Helper.Random.Next(1, 10);
            var height = Helper.Random.Next(1, 10);

            var locationCount = 50;

            var w = new WorldService(width, height);
            w.GenerateWorld(locationCount, locationCount);

            Assert.IsTrue(w.Locations.Count == locationCount);

            foreach (var l in w.Locations)
            {
                Assert.IsTrue(l != null);
            }
        }
    }
}
