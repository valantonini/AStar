using System.Drawing;
using System.Linq;
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
        
        [Test]
        public void ShouldGetCardinalSuccessorPositions()
        {
            var grid = new WorldGrid(3, 3);

            var successors = grid
                .GetSuccessorPositions(new Position(1,1))
                .ToArray();

            successors.Length.ShouldBe(4);
            
            successors[0].ShouldBe(new Position(1, 0));
            successors[1].ShouldBe(new Position(2, 1));
            successors[2].ShouldBe(new Position(1, 2));
            successors[3].ShouldBe(new Position(0, 1));
        }
        
        [Test]
        public void ShouldGetCardinalAndDiagonalSuccessorPositions()
        {
            var grid = new WorldGrid(3, 3);

            var successors = grid
                .GetSuccessorPositions(new Position(1,1), true)
                .ToArray();

            successors.Length.ShouldBe(8);
            
            successors[0].ShouldBe(new Position(1, 0));
            successors[1].ShouldBe(new Position(2, 1));
            successors[2].ShouldBe(new Position(1, 2));
            successors[3].ShouldBe(new Position(0, 1));
            
            successors[4].ShouldBe(new Position(2, 0));
            successors[5].ShouldBe(new Position(2, 2));
            successors[6].ShouldBe(new Position(0, 2));
            successors[7].ShouldBe(new Position(0, 0));
        }
        
        [Test]
        public void ShouldGetSuccessorsWithoutGoingOutOfBounds()
        {
            var grid = new WorldGrid(3, 3);

            var successors = grid
                .GetSuccessorPositions(new Position(2,2), true)
                .ToArray();

            successors.Length.ShouldBe(3);
            
            successors[0].ShouldBe(new Position(2, 1));
            successors[1].ShouldBe(new Position(1, 2));
            successors[2].ShouldBe(new Position(1, 1));
        }
    }
}