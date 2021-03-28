using System;
using AStar.Collections;
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
            Console.WriteLine(Helper.PrintGrid(grid));
            Console.WriteLine(Helper.PrintPath(grid, path));

            path[0].Row.ShouldBe(2);
            path[0].Column.ShouldBe(4);
            path[1].Row.ShouldBe(2);
            path[1].Column.ShouldBe(3);
            path[2].Row.ShouldBe(2);
            path[2].Column.ShouldBe(2);
            path[3].Row.ShouldBe(1);
            path[3].Column.ShouldBe(1);
            path[4].Row.ShouldBe(0);
            path[4].Column.ShouldBe(0);
        }

        [Test]
        public void ShouldPathToSelf()
        {
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(1, 1));
            path.Length.ShouldBe(1);

            var node = path[0];
            node.Row.ShouldBe(1);
            node.Column.ShouldBe(1);
        }

        [Test]
        public void ShouldPathToAdjacent()
        {
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(2, 1));

            path.Length.ShouldBe(2);

            var node = path[0];

            node.Row.ShouldBe(2);
            node.Column.ShouldBe(1);


            node = path[1];
            node.Row.ShouldBe(1);
            node.Column.ShouldBe(1);


            Console.WriteLine(Helper.PrintGrid(_world));
            Console.WriteLine(Helper.PrintPath(_world, path));
        }

        [Test]
        public void ShouldDoSimplePath()
        {
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            Helper.Print(_world, path);
            path.Length.ShouldBe(4);

            var item = path[3];
            item.Row.ShouldBe(1);
            item.Column.ShouldBe(1);

            item = path[2];
            item.Row.ShouldBe(2);
            item.Column.ShouldBe(2);

            item = path[1];
            item.Row.ShouldBe(3);
            item.Column.ShouldBe(2);

            item = path[0];
            item.Row.ShouldBe(4);
            item.Column.ShouldBe(2);
        }

        [Test]
        public void ShouldDoSimplePathWithNoDiagonal()
        {
            var pathfinderOptions = new PathFinderOptions { DiagonalOptions = DiagonalOptions.NoDiagonals };
            _pathFinder = new PathFinder(_world, pathfinderOptions);

            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            Helper.Print(_world, path);
            PrintCoordinates(path);

            path.Length.ShouldBe(5);

            var item = path[4];
            item.Row.ShouldBe(1);
            item.Column.ShouldBe(1);

            item = path[3];
            item.Row.ShouldBe(2);
            item.Column.ShouldBe(1);

            item = path[2];
            item.Row.ShouldBe(3);
            item.Column.ShouldBe(1);

            item = path[1];
            item.Row.ShouldBe(4);
            item.Column.ShouldBe(1);

            item = path[0];
            item.Row.ShouldBe(4);
            item.Column.ShouldBe(2);
        }

        [Test]
        public void ShouldDoSimplePathWithNoDiagonalAroundObstacle()
        {
            var pathfinderOptions = new PathFinderOptions { DiagonalOptions = DiagonalOptions.NoDiagonals };
            _pathFinder = new PathFinder(_world, pathfinderOptions);

            _world[2, 0] = 0;
            _world[2, 1] = 0;
            _world[2, 2] = 0;

            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            Helper.Print(_world, path);
            PrintCoordinates(path);

            path.Length.ShouldBe(7);

            var item = path[6];
            item.Row.ShouldBe(1);
            item.Column.ShouldBe(1);

            item = path[5];
            item.Row.ShouldBe(1);
            item.Column.ShouldBe(2);

            item = path[4];
            item.Row.ShouldBe(1);
            item.Column.ShouldBe(3);

            item = path[3];
            item.Row.ShouldBe(2);
            item.Column.ShouldBe(3);

            item = path[2];
            item.Row.ShouldBe(3);
            item.Column.ShouldBe(3);

            item = path[1];
            item.Row.ShouldBe(3);
            item.Column.ShouldBe(2);

            item = path[0];
            item.Row.ShouldBe(4);
            item.Column.ShouldBe(2);

        }
        [Test]
        public void ShouldPathAroundObstacle()
        {
            _world[2, 0] = 0;
            _world[2, 1] = 0;
            _world[2, 2] = 0;
            _world[2, 3] = 0;
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            Helper.Print(_world, path);
            path.Length.ShouldBe(6);

            var item = path[5];
            item.Row.ShouldBe(1);
            item.Column.ShouldBe(1);

            item = path[4];
            item.Row.ShouldBe(1);
            item.Column.ShouldBe(2);

            item = path[3];
            item.Row.ShouldBe(1);
            item.Column.ShouldBe(3);

            item = path[2];
            item.Row.ShouldBe(2);
            item.Column.ShouldBe(4);

            item = path[1];
            item.Row.ShouldBe(3);
            item.Column.ShouldBe(3);

            item = path[0];
            item.Row.ShouldBe(4);
            item.Column.ShouldBe(2);
        }

        [Test]
        public void ShouldFindNoPath()
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
            path.ShouldBe(null);
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
