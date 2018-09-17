using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class TestPathing
    {
        private byte[,] _grid;

        [SetUp]
        public void SetUp()
        {
            var level = @"XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                          XOOOXXXXOOOOOOOOOOOOOOOOOOOOOOOX
                          XOOOXXXXOOOOOOOOOOOOOOOOOOOOOOOX
                          XOOOXXXXOOOOOOOOOOOOOOOOOOOOOOOX
                          XOOOXXXXOOOOOOOOOOOOOOOOOOOOOOOX
                          XOOOXXXXOOOOOOOOOOOOOOOOOOOOOOOX
                          XOOOXXXXOOOOOOOOOOOOOOOOOOOOOOOX
                          XOOOXXXXOOOOOOOOOOOOOOOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOX
                          XXXXXXXXXXXXXXXXXXXXXXOOOOOOOOOX
                          XXXXXXXXXXXXXXXXXXXXXXOOOOOOOOOX
                          XXXXXXXXXXXXXXXXXXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOXXXXXOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOX
                          XOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOX
                          XOOOOOOXXXXXXXXXXXXXXXXXXXXXXXXX
                          XOOOOOOXXXXXXXXXXXXXXXXXXXXXXXXX
                          XOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOX
                          XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

            _grid = new byte[32, 32];
            var splitLevel = level.Split('\n')
                                  .Select(x => x.Trim())
                                  .ToList();

            for (var x = 0; x < splitLevel.Count; x++)
            {
                for (var y = 0; y < splitLevel[x].Length; y++)
                {
                    if (splitLevel[x][y] != 'X')
                    {
                        _grid[x, y] = 1;
                    }

                }
            }

        }

        [Test]
        public void TestPathingOptions()
        {
            var pathfinderOptions = new PathFinderOptions { PunishChangeDirection = true };

            var pathfinder = new PathFinder(_grid, pathfinderOptions);
            var path = pathfinder.FindPath(new Point(1, 1), new Point(30, 30));
            Helper.Print(_grid, path);
        }

        [Test]
        public void TestPathingEnvironment()
        {
            var pathfinder = new PathFinder(_grid);
            var path = pathfinder.FindPath(new Point(1, 1), new Point(30, 30));
            Helper.Print(_grid, path);

            path[0].Column.ShouldBe(30);
            path[0].Row.ShouldBe(30);
            path[1].Column.ShouldBe(30);
            path[1].Row.ShouldBe(29);
            path[2].Column.ShouldBe(30);
            path[2].Row.ShouldBe(28);
            path[3].Column.ShouldBe(30);
            path[3].Row.ShouldBe(27);
            path[4].Column.ShouldBe(30);
            path[4].Row.ShouldBe(26);
            path[5].Column.ShouldBe(30);
            path[5].Row.ShouldBe(25);
            path[6].Column.ShouldBe(30);
            path[6].Row.ShouldBe(24);
            path[7].Column.ShouldBe(30);
            path[7].Row.ShouldBe(23);
            path[8].Column.ShouldBe(30);
            path[8].Row.ShouldBe(22);
            path[9].Column.ShouldBe(30);
            path[9].Row.ShouldBe(21);
            path[10].Column.ShouldBe(30);
            path[10].Row.ShouldBe(20);
            path[11].Column.ShouldBe(30);
            path[11].Row.ShouldBe(19);
            path[12].Column.ShouldBe(30);
            path[12].Row.ShouldBe(18);
            path[13].Column.ShouldBe(30);
            path[13].Row.ShouldBe(17);
            path[14].Column.ShouldBe(30);
            path[14].Row.ShouldBe(16);
            path[15].Column.ShouldBe(30);
            path[15].Row.ShouldBe(15);
            path[16].Column.ShouldBe(30);
            path[16].Row.ShouldBe(14);
            path[17].Column.ShouldBe(30);
            path[17].Row.ShouldBe(13);
            path[18].Column.ShouldBe(30);
            path[18].Row.ShouldBe(12);
            path[19].Column.ShouldBe(30);
            path[19].Row.ShouldBe(11);
            path[20].Column.ShouldBe(30);
            path[20].Row.ShouldBe(10);
            path[21].Column.ShouldBe(30);
            path[21].Row.ShouldBe(9);
            path[22].Column.ShouldBe(30);
            path[22].Row.ShouldBe(8);
            path[23].Column.ShouldBe(30);
            path[23].Row.ShouldBe(7);
            path[24].Column.ShouldBe(29);
            path[24].Row.ShouldBe(6);
            path[25].Column.ShouldBe(28);
            path[25].Row.ShouldBe(6);
            path[26].Column.ShouldBe(27);
            path[26].Row.ShouldBe(7);
            path[27].Column.ShouldBe(27);
            path[27].Row.ShouldBe(8);
            path[28].Column.ShouldBe(27);
            path[28].Row.ShouldBe(9);
            path[29].Column.ShouldBe(27);
            path[29].Row.ShouldBe(10);
            path[30].Column.ShouldBe(27);
            path[30].Row.ShouldBe(11);
            path[31].Column.ShouldBe(27);
            path[31].Row.ShouldBe(12);
            path[32].Column.ShouldBe(27);
            path[32].Row.ShouldBe(13);
            path[33].Column.ShouldBe(27);
            path[33].Row.ShouldBe(14);
            path[34].Column.ShouldBe(27);
            path[34].Row.ShouldBe(15);
            path[35].Column.ShouldBe(27);
            path[35].Row.ShouldBe(16);
            path[36].Column.ShouldBe(27);
            path[36].Row.ShouldBe(17);
            path[37].Column.ShouldBe(27);
            path[37].Row.ShouldBe(18);
            path[38].Column.ShouldBe(27);
            path[38].Row.ShouldBe(19);
            path[39].Column.ShouldBe(27);
            path[39].Row.ShouldBe(20);
            path[40].Column.ShouldBe(26);
            path[40].Row.ShouldBe(21);
            path[41].Column.ShouldBe(25);
            path[41].Row.ShouldBe(22);
            path[42].Column.ShouldBe(24);
            path[42].Row.ShouldBe(23);
            path[43].Column.ShouldBe(23);
            path[43].Row.ShouldBe(24);
            path[44].Column.ShouldBe(22);
            path[44].Row.ShouldBe(25);
            path[45].Column.ShouldBe(21);
            path[45].Row.ShouldBe(26);
            path[46].Column.ShouldBe(20);
            path[46].Row.ShouldBe(27);
            path[47].Column.ShouldBe(19);
            path[47].Row.ShouldBe(28);
            path[48].Column.ShouldBe(18);
            path[48].Row.ShouldBe(29);
            path[49].Column.ShouldBe(17);
            path[49].Row.ShouldBe(28);
            path[50].Column.ShouldBe(16);
            path[50].Row.ShouldBe(27);
            path[51].Column.ShouldBe(15);
            path[51].Row.ShouldBe(26);
            path[52].Column.ShouldBe(14);
            path[52].Row.ShouldBe(25);
            path[53].Column.ShouldBe(13);
            path[53].Row.ShouldBe(24);
            path[54].Column.ShouldBe(12);
            path[54].Row.ShouldBe(23);
            path[55].Column.ShouldBe(11);
            path[55].Row.ShouldBe(22);
            path[56].Column.ShouldBe(10);
            path[56].Row.ShouldBe(21);
            path[57].Column.ShouldBe(10);
            path[57].Row.ShouldBe(20);
            path[58].Column.ShouldBe(10);
            path[58].Row.ShouldBe(19);
            path[59].Column.ShouldBe(10);
            path[59].Row.ShouldBe(18);
            path[60].Column.ShouldBe(10);
            path[60].Row.ShouldBe(17);
            path[61].Column.ShouldBe(10);
            path[61].Row.ShouldBe(16);
            path[62].Column.ShouldBe(10);
            path[62].Row.ShouldBe(15);
            path[63].Column.ShouldBe(10);
            path[63].Row.ShouldBe(14);
            path[64].Column.ShouldBe(10);
            path[64].Row.ShouldBe(13);
            path[65].Column.ShouldBe(10);
            path[65].Row.ShouldBe(12);
            path[66].Column.ShouldBe(10);
            path[66].Row.ShouldBe(11);
            path[67].Column.ShouldBe(10);
            path[67].Row.ShouldBe(10);
            path[68].Column.ShouldBe(10);
            path[68].Row.ShouldBe(9);
            path[69].Column.ShouldBe(10);
            path[69].Row.ShouldBe(8);
            path[70].Column.ShouldBe(10);
            path[70].Row.ShouldBe(7);
            path[71].Column.ShouldBe(10);
            path[71].Row.ShouldBe(6);
            path[72].Column.ShouldBe(9);
            path[72].Row.ShouldBe(5);
            path[73].Column.ShouldBe(8);
            path[73].Row.ShouldBe(4);
            path[74].Column.ShouldBe(7);
            path[74].Row.ShouldBe(3);
            path[75].Column.ShouldBe(6);
            path[75].Row.ShouldBe(3);
            path[76].Column.ShouldBe(5);
            path[76].Row.ShouldBe(3);
            path[77].Column.ShouldBe(4);
            path[77].Row.ShouldBe(3);
            path[78].Column.ShouldBe(3);
            path[78].Row.ShouldBe(3);
            path[79].Column.ShouldBe(2);
            path[79].Row.ShouldBe(2);
            path[80].Column.ShouldBe(1);
            path[80].Row.ShouldBe(1);

        }
    }
}
