using System;
using AStar.Options;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class PathfinderTests
    {
        private WorldGrid _world;
        private PathFinder _pathFinder;

        [SetUp]
        public void SetUp()
        {
            _world = CreateGridInitializedToOpen(8, 8);
            _pathFinder = new PathFinder(_world);
        }

        [Test]
        public void ShouldPathRectangleGrid()
        {
            var grid = CreateGridInitializedToOpen(3, 5);
            var pathfinder = new PathFinder(grid);

            var path = pathfinder.FindPath(new Position(0, 0), new Position(2, 4));
            
            path.ShouldBe(new[] {
                new Position(0, 0),
                new Position(1, 1),
                new Position(2, 2),
                new Position(2, 3),
                new Position(2, 4),
            });
        }

        [Test]
        public void ShouldPathToSelf()
        {
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(1, 1));

            path.ShouldBe(new[] {
                new Position(1, 1),
            });
        }

        [Test]
        public void ShouldPathToAdjacent()
        {
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(2, 1));
            
            path.ShouldBe(new[] {
                new Position(1, 1),
                new Position(2, 1),
            });
        }

        [Test]
        public void ShouldDoSimplePath()
        {
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            
            path.ShouldBe(new[] {
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 2),
                new Position(4, 2),
            });
        }

        [Test]
        public void ShouldDoSimplePathWithNoDiagonal()
        {
            var pathfinderOptions = new PathFinderOptions { UseDiagonals = false };
            _pathFinder = new PathFinder(_world, pathfinderOptions);

            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            
            path.ShouldBe(new[] {
                new Position(1, 1),
                new Position(2, 1),
                new Position(3, 1),
                new Position(4, 1),
                new Position(4, 2),
            });
        }

        [Test]
        public void ShouldDoSimplePathWithNoDiagonalAroundObstacle()
        {
            var pathfinderOptions = new PathFinderOptions { UseDiagonals = false };
            _pathFinder = new PathFinder(_world, pathfinderOptions);

            _world[2, 0] = 0;
            _world[2, 1] = 0;
            _world[2, 2] = 0;

            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            
            path.ShouldBe(new[] {
                new Position(1, 1),
                new Position(1, 2),
                new Position(1, 3),
                new Position(2, 3),
                new Position(3, 3),
                new Position(3, 2),
                new Position(4, 2),
            });
        }
        [Test]
        public void ShouldPathAroundObstacle()
        {
            _world[2, 0] = 0;
            _world[2, 1] = 0;
            _world[2, 2] = 0;
            _world[2, 3] = 0;
            
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            
            path.ShouldBe(new[] {
                new Position(1, 1),
                new Position(1, 2),
                new Position(1, 3),
                new Position(2, 4),
                new Position(3, 3),
                new Position(4, 2),
            });
        }

        [Test]
        public void ShouldReturnEmptyPathIfUnreachable()
        {
            _world[2, 0] = 0;
            _world[2, 1] = 0;
            _world[2, 2] = 0;
            _world[2, 3] = 0;
            _world[2, 4] = 0;
            _world[2, 5] = 0;
            _world[2, 6] = 0;
            _world[2, 7] = 0;
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            path.ShouldBeEmpty();
        }

        private static WorldGrid CreateGridInitializedToOpen(int height, int width)
        {
            var grid = new WorldGrid(height, width);

            for (var row = 0; row < grid.Height; row++)
            {
                for (var column = 0; column < grid.Width; column++)
                {
                    grid[row, column] = 1;
                }
            }

            return grid;
        }

        private static void PrintCoordinates(Position[] path)
        {
            foreach (var node in path)
            {
                Console.WriteLine(node.Row);
                Console.WriteLine(node.Column);
                Console.WriteLine(Environment.NewLine);
            }
        }
    }
}
