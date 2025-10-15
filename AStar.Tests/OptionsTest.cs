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

            path.ShouldBeEmpty();
        }

        [Test]
        public void ShouldStartOnClosed()
        {
            var pathfinder = new PathFinder(_world, new PathFinderOptions { UseDiagonals = false });

            var path = pathfinder.FindPath(new Position(0, 1), new Position(2, 3));

            path.ShouldBe(new[] {
                new Position(0, 1),
                new Position(1, 1),
                new Position(2, 1),
                new Position(2, 2),
                new Position(2, 3),
            });
        }
        
        [Test]
        public void ShouldEndOnClosed()
        {
            var pathfinder = new PathFinder(_world, new PathFinderOptions { IgnoreClosedEndCell = true });

            var path = pathfinder.FindPath(new Position(1, 0), new Position(1, 3));

            path.ShouldBe(new[] {
                new Position(1, 0),
                new Position(1, 1),
                new Position(1, 2),
                new Position(1, 3),
            });
        }
    }
}
