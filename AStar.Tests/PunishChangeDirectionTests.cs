using AStar.Options;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class PunishChangeDirectionTests
    {
        private WorldGrid _world;

        [SetUp]
        public void SetUp()
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

            _world = Helper.ConvertStringToPathfinderGrid(level);
        }
        
        [Test]
        public void ShouldPunishChangingDirections()
        {
            var pathFinderOptions = new PathFinderOptions { UseDiagonals = true, PunishChangeDirection = true };
            var pathfinder = new PathFinder(_world, pathFinderOptions);

            var path = pathfinder.FindPath(new Position(2, 9), new Position(15, 3));

            Helper.Print(_world, path);
            
            path.ShouldBe(new[] {
                new Position(2, 9),
                new Position(2, 8),
                new Position(2, 7),
                new Position(3, 6),
                new Position(4, 6),
                new Position(5, 6),
                new Position(6, 6),
                new Position(7, 6),
                new Position(8, 6),
                new Position(9, 6),
                new Position(10, 6),
                new Position(11, 6),
                new Position(12, 5),
                new Position(12, 4),
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

            _world = Helper.ConvertStringToPathfinderGrid(level);
            var pathfinder = new PathFinder(_world, new PathFinderOptions { UseDiagonals = false, PunishChangeDirection = true});

            var path = pathfinder.FindPath(new Position(4, 4), new Position(1, 1));

            Helper.Print(_world, path);
            
            path.ShouldBe(new[] {
                new Position(4, 4),
                new Position(3, 4),
                new Position(3, 3),
                new Position(3, 2),
                new Position(3, 1),
                new Position(3, 0),
                new Position(2, 0),
                new Position(1, 0),
                new Position(1, 1),
            });
        }
        
    }
}