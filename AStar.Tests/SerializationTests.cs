using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AStar.Heuristics;
using AStar.Options;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class SerializationTests
    {
        private PathFinder _pathfinder;

        [SetUp]
        public void SetUp()
        {
            var level = @"12
                          34
                          56";

            var world = Helper.ConvertStringToPathfinderGrid(level);

            var formatter = new BinaryFormatter();
            var stream = new System.IO.MemoryStream();
            var options = new Options.PathFinderOptions
            {
                UseDiagonals = false,
                HeuristicFormula = Heuristics.HeuristicFormula.DiagonalShortCut,
                PunishChangeDirection = true,
                SearchLimit = 314,
            };

            var pathfinder = new PathFinder(world, options);
            formatter.Serialize(stream, pathfinder);
            stream.Flush();

            stream.Seek(0, SeekOrigin.Begin);
            formatter = new BinaryFormatter();
            _pathfinder = formatter.Deserialize(stream) as PathFinder;
        }

        [Test]
        public void TestWorldSerialization()
        {
            var grid = _pathfinder
                            .GetType()
                            .GetField("_world", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            .GetValue(_pathfinder) as WorldGrid;

            grid.Width.ShouldBe(2);
            grid.Height.ShouldBe(3);

            grid[0, 0].ShouldBe((short)1);
            grid[0, 1].ShouldBe((short)2);

            grid[1, 0].ShouldBe((short)3);
            grid[1, 1].ShouldBe((short)4);


            grid[2, 0].ShouldBe((short)5);
            grid[2, 1].ShouldBe((short)6);
        }

        [Test]
        public void TestHeuristicSerialization()
        {
            var heuristic = _pathfinder
                                .GetType()
                                .GetField("_heuristic", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                .GetValue(_pathfinder);

            heuristic.ShouldBeOfType<DiagonalShortcut>();
        }

        [Test]
        public void TestOptionsSerialization()
        {
            var options = _pathfinder
                        .GetType()
                        .GetField("_options", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        .GetValue(_pathfinder) as PathFinderOptions;

            options.UseDiagonals.ShouldBeFalse();
            options.HeuristicFormula.ShouldBe(HeuristicFormula.DiagonalShortCut);
            options.PunishChangeDirection.ShouldBeTrue();
            options.SearchLimit.ShouldBe(314);
        }
    }
}
