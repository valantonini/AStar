using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class LongerPathingTests
    {
        private PathfinderGrid _pathfinderGrid;

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

            _pathfinderGrid = Helper.ConvertStringToPathfinderGrid(level);
        }

        [Test]
        public void TestPathingOptions()
        {
            var pathfinderOptions = new PathFinderOptions { PunishChangeDirection = true };

            var pathfinder = new PathFinder(_pathfinderGrid, pathfinderOptions);
            var path = pathfinder.FindPath(new Position(1, 1), new Position(30, 30));
            Helper.Print(_pathfinderGrid, path);
        }

        [Test]
        public void ShouldPathEnvironment()
        {
            var pathfinder = new PathFinder(_pathfinderGrid);
            var path = pathfinder.FindPath(new Position(1, 1), new Position(30, 30));
            Helper.Print(_pathfinderGrid, path);

            path[0].Position.Row.ShouldBe(30);
            path[0].Position.Column.ShouldBe(30);
            path[1].Position.Row.ShouldBe(30);
            path[1].Position.Column.ShouldBe(29);
            path[2].Position.Row.ShouldBe(30);
            path[2].Position.Column.ShouldBe(28);
            path[3].Position.Row.ShouldBe(30);
            path[3].Position.Column.ShouldBe(27);
            path[4].Position.Row.ShouldBe(30);
            path[4].Position.Column.ShouldBe(26);
            path[5].Position.Row.ShouldBe(30);
            path[5].Position.Column.ShouldBe(25);
            path[6].Position.Row.ShouldBe(30);
            path[6].Position.Column.ShouldBe(24);
            path[7].Position.Row.ShouldBe(30);
            path[7].Position.Column.ShouldBe(23);
            path[8].Position.Row.ShouldBe(30);
            path[8].Position.Column.ShouldBe(22);
            path[9].Position.Row.ShouldBe(30);
            path[9].Position.Column.ShouldBe(21);
            path[10].Position.Row.ShouldBe(30);
            path[10].Position.Column.ShouldBe(20);
            path[11].Position.Row.ShouldBe(30);
            path[11].Position.Column.ShouldBe(19);
            path[12].Position.Row.ShouldBe(30);
            path[12].Position.Column.ShouldBe(18);
            path[13].Position.Row.ShouldBe(30);
            path[13].Position.Column.ShouldBe(17);
            path[14].Position.Row.ShouldBe(30);
            path[14].Position.Column.ShouldBe(16);
            path[15].Position.Row.ShouldBe(30);
            path[15].Position.Column.ShouldBe(15);
            path[16].Position.Row.ShouldBe(30);
            path[16].Position.Column.ShouldBe(14);
            path[17].Position.Row.ShouldBe(30);
            path[17].Position.Column.ShouldBe(13);
            path[18].Position.Row.ShouldBe(30);
            path[18].Position.Column.ShouldBe(12);
            path[19].Position.Row.ShouldBe(30);
            path[19].Position.Column.ShouldBe(11);
            path[20].Position.Row.ShouldBe(30);
            path[20].Position.Column.ShouldBe(10);
            path[21].Position.Row.ShouldBe(30);
            path[21].Position.Column.ShouldBe(9);
            path[22].Position.Row.ShouldBe(30);
            path[22].Position.Column.ShouldBe(8);
            path[23].Position.Row.ShouldBe(30);
            path[23].Position.Column.ShouldBe(7);
            path[24].Position.Row.ShouldBe(29);
            path[24].Position.Column.ShouldBe(6);
            path[25].Position.Row.ShouldBe(28);
            path[25].Position.Column.ShouldBe(6);
            path[26].Position.Row.ShouldBe(27);
            path[26].Position.Column.ShouldBe(7);
            path[27].Position.Row.ShouldBe(27);
            path[27].Position.Column.ShouldBe(8);
            path[28].Position.Row.ShouldBe(27);
            path[28].Position.Column.ShouldBe(9);
            path[29].Position.Row.ShouldBe(27);
            path[29].Position.Column.ShouldBe(10);
            path[30].Position.Row.ShouldBe(27);
            path[30].Position.Column.ShouldBe(11);
            path[31].Position.Row.ShouldBe(27);
            path[31].Position.Column.ShouldBe(12);
            path[32].Position.Row.ShouldBe(27);
            path[32].Position.Column.ShouldBe(13);
            path[33].Position.Row.ShouldBe(27);
            path[33].Position.Column.ShouldBe(14);
            path[34].Position.Row.ShouldBe(27);
            path[34].Position.Column.ShouldBe(15);
            path[35].Position.Row.ShouldBe(27);
            path[35].Position.Column.ShouldBe(16);
            path[36].Position.Row.ShouldBe(27);
            path[36].Position.Column.ShouldBe(17);
            path[37].Position.Row.ShouldBe(27);
            path[37].Position.Column.ShouldBe(18);
            path[38].Position.Row.ShouldBe(27);
            path[38].Position.Column.ShouldBe(19);
            path[39].Position.Row.ShouldBe(27);
            path[39].Position.Column.ShouldBe(20);
            path[40].Position.Row.ShouldBe(26);
            path[40].Position.Column.ShouldBe(21);
            path[41].Position.Row.ShouldBe(25);
            path[41].Position.Column.ShouldBe(22);
            path[42].Position.Row.ShouldBe(24);
            path[42].Position.Column.ShouldBe(23);
            path[43].Position.Row.ShouldBe(23);
            path[43].Position.Column.ShouldBe(24);
            path[44].Position.Row.ShouldBe(22);
            path[44].Position.Column.ShouldBe(25);
            path[45].Position.Row.ShouldBe(21);
            path[45].Position.Column.ShouldBe(26);
            path[46].Position.Row.ShouldBe(20);
            path[46].Position.Column.ShouldBe(27);
            path[47].Position.Row.ShouldBe(19);
            path[47].Position.Column.ShouldBe(28);
            path[48].Position.Row.ShouldBe(18);
            path[48].Position.Column.ShouldBe(29);
            path[49].Position.Row.ShouldBe(17);
            path[49].Position.Column.ShouldBe(28);
            path[50].Position.Row.ShouldBe(16);
            path[50].Position.Column.ShouldBe(27);
            path[51].Position.Row.ShouldBe(15);
            path[51].Position.Column.ShouldBe(26);
            path[52].Position.Row.ShouldBe(14);
            path[52].Position.Column.ShouldBe(25);
            path[53].Position.Row.ShouldBe(13);
            path[53].Position.Column.ShouldBe(24);
            path[54].Position.Row.ShouldBe(12);
            path[54].Position.Column.ShouldBe(23);
            path[55].Position.Row.ShouldBe(11);
            path[55].Position.Column.ShouldBe(22);
            path[56].Position.Row.ShouldBe(10);
            path[56].Position.Column.ShouldBe(21);
            path[57].Position.Row.ShouldBe(10);
            path[57].Position.Column.ShouldBe(20);
            path[58].Position.Row.ShouldBe(10);
            path[58].Position.Column.ShouldBe(19);
            path[59].Position.Row.ShouldBe(10);
            path[59].Position.Column.ShouldBe(18);
            path[60].Position.Row.ShouldBe(10);
            path[60].Position.Column.ShouldBe(17);
            path[61].Position.Row.ShouldBe(10);
            path[61].Position.Column.ShouldBe(16);
            path[62].Position.Row.ShouldBe(10);
            path[62].Position.Column.ShouldBe(15);
            path[63].Position.Row.ShouldBe(10);
            path[63].Position.Column.ShouldBe(14);
            path[64].Position.Row.ShouldBe(10);
            path[64].Position.Column.ShouldBe(13);
            path[65].Position.Row.ShouldBe(10);
            path[65].Position.Column.ShouldBe(12);
            path[66].Position.Row.ShouldBe(10);
            path[66].Position.Column.ShouldBe(11);
            path[67].Position.Row.ShouldBe(10);
            path[67].Position.Column.ShouldBe(10);
            path[68].Position.Row.ShouldBe(10);
            path[68].Position.Column.ShouldBe(9);
            path[69].Position.Row.ShouldBe(10);
            path[69].Position.Column.ShouldBe(8);
            path[70].Position.Row.ShouldBe(10);
            path[70].Position.Column.ShouldBe(7);
            path[71].Position.Row.ShouldBe(10);
            path[71].Position.Column.ShouldBe(6);
            path[72].Position.Row.ShouldBe(9);
            path[72].Position.Column.ShouldBe(5);
            path[73].Position.Row.ShouldBe(8);
            path[73].Position.Column.ShouldBe(4);
            path[74].Position.Row.ShouldBe(7);
            path[74].Position.Column.ShouldBe(3);
            path[75].Position.Row.ShouldBe(6);
            path[75].Position.Column.ShouldBe(3);
            path[76].Position.Row.ShouldBe(5);
            path[76].Position.Column.ShouldBe(3);
            path[77].Position.Row.ShouldBe(4);
            path[77].Position.Column.ShouldBe(3);
            path[78].Position.Row.ShouldBe(3);
            path[78].Position.Column.ShouldBe(3);
            path[79].Position.Row.ShouldBe(2);
            path[79].Position.Column.ShouldBe(2);
            path[80].Position.Row.ShouldBe(1);
            path[80].Position.Column.ShouldBe(1);
        }
    }
}
