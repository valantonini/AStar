using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class SerializationTests
    {
        private WorldGrid _world;

        [SetUp]
        public void SetUp()
        {
            var level = @"12
                          34
                          56";

            _world = Helper.ConvertStringToPathfinderGrid(level);
        }

        [Test]
        public void TestWorldSerialization()
        {
            var formatter = new BinaryFormatter();  
            var stream = new System.IO.MemoryStream();  
            formatter.Serialize(stream, _world);  
            stream.Flush();

            stream.Seek(0, SeekOrigin.Begin);
            formatter = new BinaryFormatter();
            var grid = formatter.Deserialize(stream) as WorldGrid;

            grid.Width.ShouldBe(2);
            grid.Height.ShouldBe(3);

            grid[0, 0].ShouldBe((short)1);
            grid[0, 1].ShouldBe((short)2);

            grid[1, 0].ShouldBe((short)3);
            grid[1, 1].ShouldBe((short)4);


            grid[2, 0].ShouldBe((short)5);
            grid[2, 1].ShouldBe((short)6);
        }
    }
}
