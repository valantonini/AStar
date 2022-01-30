using System;
using AStar.Heuristics;
using AStar.Options;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class PunishChangeDirectionTests
    {
        [Test]
        public void ShouldPunishChangingDirections()
        {
            var level = @"  111111111X
                            111111111X
                            11111X111X
                            11111X111X
                            11111X111X
                            111111111X
                            111111111X
                            111111111X
                            111XXX111X
                            111111111X
                            111X1X111X
                            111111111X
                            111111111X
                            1111XX111X
                            11XXXX111X
                            1111XXX11X
                            1111XXX11X
                            1111XXX11X
                            111111111X
                            111111111X";

            var world = Helper.ConvertStringToPathfinderGrid(level);

            var pathFinderOptions = new PathFinderOptions { UseDiagonals = true, PunishChangeDirection = true};
            var pathfinder = new PathFinder(world, pathFinderOptions);

            var path = pathfinder.FindPath(new Position(2, 9), new Position(15, 3));


            Helper.Print(world, path);

            path.ShouldBe(new[]
            {
                new Position(2, 9),
                new Position(3, 8),
                new Position(4, 7),
                new Position(5, 6),
                new Position(6, 5),
                new Position(7, 5),
                new Position(8, 6),
                new Position(9, 5),
                new Position(10, 4),
                new Position(11, 3),
                new Position(12, 3),
                new Position(13, 2),
                new Position(14, 1),
                new Position(15, 2),
                new Position(15, 3),
            });
        }

        [Test]
        public void ShouldCalculateAdjacentCorrectly()
        {
            var level = @"  110111
                            110111
                            100111
                            111111
                            101111
                            111111";

            var world = Helper.ConvertStringToPathfinderGrid(level);
            var pathfinder = new PathFinder(world, new PathFinderOptions { UseDiagonals = false, PunishChangeDirection = true, HeuristicFormula = HeuristicFormula.MaxDXDY });

            var path = pathfinder.FindPath(new Position(4, 4), new Position(1, 1));

            var expected = new[]
            {
                new Position(4, 4),
                new Position(3, 4),
                new Position(3, 3),
                new Position(3, 2),
                new Position(3, 1),
                new Position(3, 0),
                new Position(2, 0),
                new Position(1, 0),
                new Position(1, 1),
            };

            Console.WriteLine("actual");
            Helper.Print(world, path);
            Console.WriteLine("expected");
            Helper.Print(world, expected);

            path.ShouldBe(expected);
        }
    }
}