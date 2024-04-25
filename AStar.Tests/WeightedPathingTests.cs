using AStar.Options;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
  [TestFixture]
  public class WeightedPathingTests
  {
    [Test]
    public void ShouldPathWithWeight()
    {
        var level = @"1111115
                      1511151
                      1155511
                      1111111";
        var world = Helper.ConvertStringToPathfinderGrid(level);
        var opts = new PathFinderOptions { Weighting = Weighting.Positive };
        var pathfinder = new PathFinder(world, opts);

        var path = pathfinder.FindPath(new Position(1, 1), new Position(1, 5));

        path.ShouldBe(new[] {
              new Position(1, 1),
              new Position(2, 2),
              new Position(2, 3),
              new Position(2, 4),
              new Position(1, 5),
            });
    }

    [Test]
    public void ShouldPathWithInvertedWeight()
    {
      var level = @"9999995
                    9599959
                    9955599
                    9999999";

        var world = Helper.ConvertStringToPathfinderGrid(level);
        var opts = new PathFinderOptions { Weighting = Weighting.Negative };
        var pathfinder = new PathFinder(world, opts);

        var path = pathfinder.FindPath(new Position(1, 1), new Position(1, 5));

        path.ShouldBe(new[] {
              new Position(1, 1),
              new Position(2, 2),
              new Position(2, 3),
              new Position(2, 4),
              new Position(1, 5),
            });
    }
  }
}
