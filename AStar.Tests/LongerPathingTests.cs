using AStar.Options;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class LongerPathingTests
    {
        private WorldGrid _world;

        [SetUp]
        public void SetUp()
        {
            var level = @"XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111111111111111111111111111111X
                          X111111111111111111111111111111X
                          X111111111111111111111111111111X
                          XXXXXXXXXXXXXXXXXXXXXX111111111X
                          XXXXXXXXXXXXXXXXXXXXXX111111111X
                          XXXXXXXXXXXXXXXXXXXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X111111111111111111111111111111X
                          X111111111111111111111111111111X
                          X111111XXXXXXXXXXXXXXXXXXXXXXXXX
                          X111111XXXXXXXXXXXXXXXXXXXXXXXXX
                          X111111111111111111111111111111X
                          XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

            _world = Helper.ConvertStringToPathfinderGrid(level);
        }

        [Test]
        public void TestPathingOptions()
        {
            var pathfinderOptions = new PathFinderOptions { PunishChangeDirection = true };

            var pathfinder = new PathFinder(_world, pathfinderOptions);
            var path = pathfinder.FindPath(new Position(1, 1), new Position(30, 30));
            
        }

        [Test]
        public void ShouldPathEnvironment()
        {
            var pathfinder = new PathFinder(_world);
            
            var path = pathfinder.FindPath(new Position(1, 1), new Position(30, 30));
            
            

            path.ShouldBe(new[] {
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 3),
                new Position(5, 3),
                new Position(6, 3),
                new Position(7, 3),
                new Position(8, 4),
                new Position(9, 5),
                new Position(10, 6),
                new Position(10, 7),
                new Position(10, 8),
                new Position(10, 9),
                new Position(10, 10),
                new Position(10, 11),
                new Position(10, 12),
                new Position(10, 13),
                new Position(10, 14),
                new Position(10, 15),
                new Position(10, 16),
                new Position(10, 17),
                new Position(10, 18),
                new Position(10, 19),
                new Position(10, 20),
                new Position(10, 21),
                new Position(11, 22),
                new Position(12, 23),
                new Position(13, 24),
                new Position(14, 25),
                new Position(15, 26),
                new Position(16, 27),
                new Position(17, 28),
                new Position(18, 29),
                new Position(19, 28),
                new Position(20, 27),
                new Position(21, 26),
                new Position(22, 25),
                new Position(23, 24),
                new Position(24, 23),
                new Position(25, 22),
                new Position(26, 21),
                new Position(27, 20),
                new Position(27, 19),
                new Position(27, 18),
                new Position(27, 17),
                new Position(27, 16),
                new Position(27, 15),
                new Position(27, 14),
                new Position(27, 13),
                new Position(27, 12),
                new Position(27, 11),
                new Position(27, 10),
                new Position(27, 9),
                new Position(27, 8),
                new Position(27, 7),
                new Position(28, 6),
                new Position(29, 6),
                new Position(30, 7),
                new Position(30, 8),
                new Position(30, 9),
                new Position(30, 10),
                new Position(30, 11),
                new Position(30, 12),
                new Position(30, 13),
                new Position(30, 14),
                new Position(30, 15),
                new Position(30, 16),
                new Position(30, 17),
                new Position(30, 18),
                new Position(30, 19),
                new Position(30, 20),
                new Position(30, 21),
                new Position(30, 22),
                new Position(30, 23),
                new Position(30, 24),
                new Position(30, 25),
                new Position(30, 26),
                new Position(30, 27),
                new Position(30, 28),
                new Position(30, 29),
                new Position(30, 30),
            });
        }
    }
}
