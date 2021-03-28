using System.Drawing;
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
            var grid = new WorldGrid(12, 10);

            grid.Height.ShouldBe(12);
            grid.Width.ShouldBe(10);
        }

        [Test]
        public void ShouldReadAndWriteByIndex()
        {
            var grid = new WorldGrid(2, 3)
            {
                [0, 0] = 1,
                [0, 1] = 2,
                [0, 2] = 3,
                [1, 0] = 4,
                [1, 1] = 5,
                [1, 2] = 6,
            };

            grid[0, 0].ShouldBe((short)1);
            grid[0, 1].ShouldBe((short)2);
            grid[0, 2].ShouldBe((short)3);

            grid[1, 0].ShouldBe((short)4);
            grid[1, 1].ShouldBe((short)5);
            grid[1, 2].ShouldBe((short)6);
        }
        
        [Test]
        public void ShouldReadAndWriteByPoint()
        {
            var grid = new WorldGrid(2, 3);
            
            grid[new Position(0, 0)] = 1;
            grid[new Position(0, 1)] = 2;
            grid[new Position(0, 2)] = 3;
            grid[new Position(1, 0)] = 4;
            grid[new Position(1, 1)] = 5;
            grid[new Position(1, 2)] = 6;
            
            grid[new Point(0, 0)].ShouldBe((short)1);
            grid[new Point(1, 0)].ShouldBe((short)2);
            grid[new Point(2, 0)].ShouldBe((short)3);
            grid[new Point(0, 1)].ShouldBe((short)4);
            grid[new Point(1, 1)].ShouldBe((short)5);
            grid[new Point(2, 1)].ShouldBe((short)6);
        }
    }
}