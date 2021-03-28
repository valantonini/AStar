using AStar.Collections;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class PathingTests
    {
        private PathfinderGrid _pathfinderGrid;

        [SetUp]
        public void SetUp()
        {
            var level = @"XXXXXXX
                          X11X11X
                          X11111X
                          XXXXXXX";

            _pathfinderGrid = Helper.ConvertStringToPathfinderGrid(level);
        }

        [Test]
        public void ShouldPathPredictably()
        {
            var pathfinder = new PathFinder(_pathfinderGrid);

            var path = pathfinder.FindPath(new Position(1, 1), new Position(2, 3));

            Helper.Print(_pathfinderGrid, path);
            Helper.PrintAssertions(path);

            path[0].Row.ShouldBe(2);
            path[0].Column.ShouldBe(3);
            path[1].Row.ShouldBe(2);
            path[1].Column.ShouldBe(2);
            path[2].Row.ShouldBe(1);
            path[2].Column.ShouldBe(1);
        }

        [Test]
        public void ShouldPathPredictably2()
        {
            var pathfinder = new PathFinder(_pathfinderGrid);

            var path = pathfinder.FindPath(new Position(1, 1), new Position(1, 5));

            Helper.Print(_pathfinderGrid, path);

            path[0].Row.ShouldBe(1);
            path[0].Column.ShouldBe(5);
            path[1].Row.ShouldBe(1);
            path[1].Column.ShouldBe(4);
            path[2].Row.ShouldBe(2);
            path[2].Column.ShouldBe(3);
            path[3].Row.ShouldBe(1);
            path[3].Column.ShouldBe(2);
            path[4].Row.ShouldBe(1);
            path[4].Column.ShouldBe(1);
        }

        [Test]
        public void ShouldPathPredictably3()
        {
            var pathfinder = new PathFinder(_pathfinderGrid, new PathFinderOptions { Diagonals = false });

            var path = pathfinder.FindPath(new Position(1, 1), new Position(1, 5));

            Helper.Print(_pathfinderGrid, path);
            Helper.PrintAssertions(path);

            path[0].Row.ShouldBe(1);
            path[0].Column.ShouldBe(5);
            path[1].Row.ShouldBe(2);
            path[1].Column.ShouldBe(5);
            path[2].Row.ShouldBe(2);
            path[2].Column.ShouldBe(4);
            path[3].Row.ShouldBe(2);
            path[3].Column.ShouldBe(3);
            path[4].Row.ShouldBe(2);
            path[4].Column.ShouldBe(2);
            path[5].Row.ShouldBe(1);
            path[5].Column.ShouldBe(2);
            path[6].Row.ShouldBe(1);
            path[6].Column.ShouldBe(1);
        }
    }
}