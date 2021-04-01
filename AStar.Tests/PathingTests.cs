using AStar.Options;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class PathingTests
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
        public void ShouldPathPredictably()
        {
            var pathfinder = new PathFinder(_world);

            var path = pathfinder.FindPath(new Position(1, 1), new Position(2, 3));

            Helper.Print(_world, path);

            var expected = new[] {
                new Position(1, 1),
                new Position(2, 2),
                new Position(2, 3),
            };
        }

        [Test]
        public void ShouldPathPredictably2()
        {
            var pathfinder = new PathFinder(_world);

            var path = pathfinder.FindPath(new Position(1, 1), new Position(1, 5));

            Helper.Print(_world, path);

            path.ShouldBe(new[] {
                new Position(1, 1),
                new Position(1, 2),
                new Position(2, 3),
                new Position(1, 4),
                new Position(1, 5),
            });

        }

        [Test]
        public void ShouldPathPredictably3()
        {
            var pathfinder = new PathFinder(_world, new PathFinderOptions { UseDiagonals = false });

            var path = pathfinder.FindPath(new Position(1, 1), new Position(1, 5));

            Helper.Print(_world, path);

            path.ShouldBe(new[] {
                new Position(1, 1),
                new Position(1, 2),
                new Position(2, 2),
                new Position(2, 3),
                new Position(2, 4),
                new Position(2, 5),
                new Position(1, 5),
            });
        }
    }
}