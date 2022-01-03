using AStar.Heuristics;
using AStar.Options;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class PunishChangeDirectionIssueTestTests
    {
        private WorldGrid _world;

        [SetUp]
        public void SetUp()
        {
            var level = @"  11111111111111111111
                            11111111111111111111
                            11111111111111X11111
                            11111111X1X111X11111
                            11111111X1111XXXXX11
                            11XXX111X1X11XXXXX11
                            111111111111111XXX11
                            11111111111111111111
                            11111111111111111111";

            _world = Helper.ConvertStringToPathfinderGrid(level);
        }
        
        [Test]
        public void ShouldPunishChangingDirectionsIssue()
        {
            var pathFinderOptions = new PathFinderOptions { UseDiagonals = false, PunishChangeDirection = false, HeuristicFormula = HeuristicFormula.Euclidean};
            var pathfinder = new PathFinder(_world, pathFinderOptions);

            var path = pathfinder.FindPath(new Position(7, 2), new Position(1, 17));

            Helper.Print(_world, path);
            
            path.ShouldBe(new[] {
                new Position(7, 2),
                new Position(7, 3),
                new Position(7, 4),
                new Position(7, 5),
                new Position(7, 6),
                new Position(7, 7),
                new Position(7, 8),
                new Position(7, 9),
                new Position(7, 10),
                new Position(7, 11),
                new Position(7, 12),
                new Position(6, 12),
                new Position(5, 12),
                new Position(4, 12),
                new Position(3, 12),
                new Position(3, 13),
                new Position(2, 13),
                new Position(1, 13),
                new Position(1, 14),
                new Position(1, 15),
                new Position(1, 16),
                new Position(1, 17),
            });
        }
        
        [Test]
        public void ShouldCorrectIssue()
        {
            var pathFinderOptions = new PathFinderOptions { UseDiagonals = true, PunishChangeDirection = false};
            var pathfinder = new PathFinder(_world, pathFinderOptions);

            var path = pathfinder.FindPath(new Position(1, 2), new Position(8, 14));

            Helper.Print(_world, path);
            
            path.ShouldBe(new[] {
                new Position(1, 2),
                new Position(2, 3),
                new Position(3, 4),
                new Position(4, 5),
                new Position(5, 6),
                new Position(6, 7),
                new Position(7, 8),
                new Position(8, 9),
                new Position(8, 10),
                new Position(8, 11),
                new Position(8, 12),
                new Position(8, 13),
                new Position(8, 14),
            });
        }
    }
}