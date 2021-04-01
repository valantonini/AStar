using AStar.Options;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class OptionsTest
    {
        private WorldGrid _world;

        [SetUp]
        public void SetUp()
        {
            var level = @"XXXXXXX
                          X11X11X
                          X11111X
                          XXXXXXX";

            _world = Helper.ConvertStringToPathfinderGrid(level);
        }

        [Test]
        public void ShouldEnforceSearchLimit()
        {
            var pathfinder = new PathFinder(_world, new PathFinderOptions { SearchLimit = 2 });

            var path = pathfinder.FindPath(new Position(1, 1), new Position(1, 5));

            Helper.Print(_world, path);
            
            path.ShouldBeEmpty();
        }
    }
}