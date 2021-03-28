using System.Drawing;
using AStar.Options;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class PointPathingTests
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
        public void ShouldPathPredictably3()
        {
            var pathfinder = new PathFinder(_world, new PathFinderOptions { DiagonalOptions = DiagonalOptions.NoDiagonals });

            var path = pathfinder.FindPath(new Point(1, 1), new Point(5, 1));

            Helper.Print(_world, path);

            path[0].X.ShouldBe(5);
            path[0].Y.ShouldBe(1);
            path[1].X.ShouldBe(5);
            path[1].Y.ShouldBe(2);
            path[2].X.ShouldBe(4);
            path[2].Y.ShouldBe(2);
            path[3].X.ShouldBe(3);
            path[3].Y.ShouldBe(2);
            path[4].X.ShouldBe(2);
            path[4].Y.ShouldBe(2);
            path[5].X.ShouldBe(2);
            path[5].Y.ShouldBe(1);
            path[6].X.ShouldBe(1);
            path[6].Y.ShouldBe(1);
        }
    }
}