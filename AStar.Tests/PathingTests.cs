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

            path[0].Position.Row.ShouldBe(2);
            path[0].Position.Column.ShouldBe(3);
            path[1].Position.Row.ShouldBe(2);
            path[1].Position.Column.ShouldBe(2);
            path[2].Position.Row.ShouldBe(1);
            path[2].Position.Column.ShouldBe(1);
        }

        [Test]
        public void ShouldPathPredictably2()
        {
            var pathfinder = new PathFinder(_pathfinderGrid);

            var path = pathfinder.FindPath(new Position(1, 1), new Position(1, 5));

            Helper.Print(_pathfinderGrid, path);

            path[0].Position.Row.ShouldBe(1);
            path[0].Position.Column.ShouldBe(5);
            path[1].Position.Row.ShouldBe(1);
            path[1].Position.Column.ShouldBe(4);
            path[2].Position.Row.ShouldBe(2);
            path[2].Position.Column.ShouldBe(3);
            path[3].Position.Row.ShouldBe(1);
            path[3].Position.Column.ShouldBe(2);
            path[4].Position.Row.ShouldBe(1);
            path[4].Position.Column.ShouldBe(1);
        }

        [Test]
        public void ShouldPathPredictably3()
        {
            var pathfinder = new PathFinder(_pathfinderGrid, new PathFinderOptions { Diagonals = false });

            var path = pathfinder.FindPath(new Position(1, 1), new Position(1, 5));

            Helper.Print(_pathfinderGrid, path);
            Helper.PrintAssertions(path);

            path[0].Position.Row.ShouldBe(1);
            path[0].Position.Column.ShouldBe(5);
            path[1].Position.Row.ShouldBe(2);
            path[1].Position.Column.ShouldBe(5);
            path[2].Position.Row.ShouldBe(2);
            path[2].Position.Column.ShouldBe(4);
            path[3].Position.Row.ShouldBe(2);
            path[3].Position.Column.ShouldBe(3);
            path[4].Position.Row.ShouldBe(2);
            path[4].Position.Column.ShouldBe(2);
            path[5].Position.Row.ShouldBe(1);
            path[5].Position.Column.ShouldBe(2);
            path[6].Position.Row.ShouldBe(1);
            path[6].Position.Column.ShouldBe(1);
        }
    }
}