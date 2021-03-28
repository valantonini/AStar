using AStar.Collections;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class GridTests
    {
        [Test]
        public void ShouldInstantiateWithCorrectDimensions()
        {
            var grid = new PathfinderGrid(12, 10);

            grid.Height.ShouldBe(12);
            grid.Width.ShouldBe(10);
        }

        [Test]
        public void ShouldReadAndWriteByIndex()
        {
            var grid = new PathfinderGrid(2, 3)
            {
                [0, 0] = 1,
                [0, 1] = 2,
                [0, 2] = 3,
                [1, 0] = 4,
                [1, 1] = 5,
                [1, 2] = 6,
            };

            grid[0, 0].ShouldBe(1);
            grid[0, 1].ShouldBe(2);
            grid[0, 2].ShouldBe(3);

            grid[1, 0].ShouldBe(4);
            grid[1, 1].ShouldBe(5);
            grid[1, 2].ShouldBe(6);
        }
    }
}