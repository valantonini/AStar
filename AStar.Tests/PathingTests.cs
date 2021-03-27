using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class PathingTests
    {
        private PathfinderGrid _pathfinderGrid;

        [SetUp]
        public void SetUp()
        {
            var level = @"XXXXXXX
                          X11X11X
                          X11111X
                          XXXXXXX";

            _pathfinderGrid = Helper.ConvertStringToPathfinderGrid(level);
        }
    }
}
