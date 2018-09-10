using System.Drawing;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace AStar.Tests
{
    public class TestPathing
    {
        private byte[,] _grid;

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

            _grid = new byte[32,32];
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

        [Fact]
        public void TestPathingOptions()
        {
            SetUp();
            var pathfinderOptions = new PathFinderOptions {PunishChangeDirection = true};

            var pathfinder = new PathFinder(_grid, pathfinderOptions);
            var path = pathfinder.FindPath(new Point(1, 1), new Point(30, 30));
            Helper.Print(_grid, path);
        }

        [Fact]
        public void TestPathingEnvironment()
        {
            SetUp();
            var pathfinder = new PathFinder(_grid);
            var path = pathfinder.FindPath(new Point(1, 1), new Point(30, 30));
            Helper.Print(_grid, path);

            path[0].X.Should().Be(30);
            path[0].Y.Should().Be(30);
            path[1].X.Should().Be(30);
            path[1].Y.Should().Be(29);
            path[2].X.Should().Be(30);
            path[2].Y.Should().Be(28);
            path[3].X.Should().Be(30);
            path[3].Y.Should().Be(27);
            path[4].X.Should().Be(30);
            path[4].Y.Should().Be(26);
            path[5].X.Should().Be(30);
            path[5].Y.Should().Be(25);
            path[6].X.Should().Be(30);
            path[6].Y.Should().Be(24);
            path[7].X.Should().Be(30);
            path[7].Y.Should().Be(23);
            path[8].X.Should().Be(30);
            path[8].Y.Should().Be(22);
            path[9].X.Should().Be(30);
            path[9].Y.Should().Be(21);
            path[10].X.Should().Be(30);
            path[10].Y.Should().Be(20);
            path[11].X.Should().Be(30);
            path[11].Y.Should().Be(19);
            path[12].X.Should().Be(30);
            path[12].Y.Should().Be(18);
            path[13].X.Should().Be(30);
            path[13].Y.Should().Be(17);
            path[14].X.Should().Be(30);
            path[14].Y.Should().Be(16);
            path[15].X.Should().Be(30);
            path[15].Y.Should().Be(15);
            path[16].X.Should().Be(30);
            path[16].Y.Should().Be(14);
            path[17].X.Should().Be(30);
            path[17].Y.Should().Be(13);
            path[18].X.Should().Be(30);
            path[18].Y.Should().Be(12);
            path[19].X.Should().Be(30);
            path[19].Y.Should().Be(11);
            path[20].X.Should().Be(30);
            path[20].Y.Should().Be(10);
            path[21].X.Should().Be(30);
            path[21].Y.Should().Be(9);
            path[22].X.Should().Be(30);
            path[22].Y.Should().Be(8);
            path[23].X.Should().Be(30);
            path[23].Y.Should().Be(7);
            path[24].X.Should().Be(29);
            path[24].Y.Should().Be(6);
            path[25].X.Should().Be(28);
            path[25].Y.Should().Be(6);
            path[26].X.Should().Be(27);
            path[26].Y.Should().Be(7);
            path[27].X.Should().Be(27);
            path[27].Y.Should().Be(8);
            path[28].X.Should().Be(27);
            path[28].Y.Should().Be(9);
            path[29].X.Should().Be(27);
            path[29].Y.Should().Be(10);
            path[30].X.Should().Be(27);
            path[30].Y.Should().Be(11);
            path[31].X.Should().Be(27);
            path[31].Y.Should().Be(12);
            path[32].X.Should().Be(27);
            path[32].Y.Should().Be(13);
            path[33].X.Should().Be(27);
            path[33].Y.Should().Be(14);
            path[34].X.Should().Be(27);
            path[34].Y.Should().Be(15);
            path[35].X.Should().Be(27);
            path[35].Y.Should().Be(16);
            path[36].X.Should().Be(27);
            path[36].Y.Should().Be(17);
            path[37].X.Should().Be(27);
            path[37].Y.Should().Be(18);
            path[38].X.Should().Be(27);
            path[38].Y.Should().Be(19);
            path[39].X.Should().Be(27);
            path[39].Y.Should().Be(20);
            path[40].X.Should().Be(26);
            path[40].Y.Should().Be(21);
            path[41].X.Should().Be(25);
            path[41].Y.Should().Be(22);
            path[42].X.Should().Be(24);
            path[42].Y.Should().Be(23);
            path[43].X.Should().Be(23);
            path[43].Y.Should().Be(24);
            path[44].X.Should().Be(22);
            path[44].Y.Should().Be(25);
            path[45].X.Should().Be(21);
            path[45].Y.Should().Be(26);
            path[46].X.Should().Be(20);
            path[46].Y.Should().Be(27);
            path[47].X.Should().Be(19);
            path[47].Y.Should().Be(28);
            path[48].X.Should().Be(18);
            path[48].Y.Should().Be(29);
            path[49].X.Should().Be(17);
            path[49].Y.Should().Be(28);
            path[50].X.Should().Be(16);
            path[50].Y.Should().Be(27);
            path[51].X.Should().Be(15);
            path[51].Y.Should().Be(26);
            path[52].X.Should().Be(14);
            path[52].Y.Should().Be(25);
            path[53].X.Should().Be(13);
            path[53].Y.Should().Be(24);
            path[54].X.Should().Be(12);
            path[54].Y.Should().Be(23);
            path[55].X.Should().Be(11);
            path[55].Y.Should().Be(22);
            path[56].X.Should().Be(10);
            path[56].Y.Should().Be(21);
            path[57].X.Should().Be(10);
            path[57].Y.Should().Be(20);
            path[58].X.Should().Be(10);
            path[58].Y.Should().Be(19);
            path[59].X.Should().Be(10);
            path[59].Y.Should().Be(18);
            path[60].X.Should().Be(10);
            path[60].Y.Should().Be(17);
            path[61].X.Should().Be(10);
            path[61].Y.Should().Be(16);
            path[62].X.Should().Be(10);
            path[62].Y.Should().Be(15);
            path[63].X.Should().Be(10);
            path[63].Y.Should().Be(14);
            path[64].X.Should().Be(10);
            path[64].Y.Should().Be(13);
            path[65].X.Should().Be(10);
            path[65].Y.Should().Be(12);
            path[66].X.Should().Be(10);
            path[66].Y.Should().Be(11);
            path[67].X.Should().Be(10);
            path[67].Y.Should().Be(10);
            path[68].X.Should().Be(10);
            path[68].Y.Should().Be(9);
            path[69].X.Should().Be(10);
            path[69].Y.Should().Be(8);
            path[70].X.Should().Be(10);
            path[70].Y.Should().Be(7);
            path[71].X.Should().Be(10);
            path[71].Y.Should().Be(6);
            path[72].X.Should().Be(9);
            path[72].Y.Should().Be(5);
            path[73].X.Should().Be(8);
            path[73].Y.Should().Be(4);
            path[74].X.Should().Be(7);
            path[74].Y.Should().Be(3);
            path[75].X.Should().Be(6);
            path[75].Y.Should().Be(3);
            path[76].X.Should().Be(5);
            path[76].Y.Should().Be(3);
            path[77].X.Should().Be(4);
            path[77].Y.Should().Be(3);
            path[78].X.Should().Be(3);
            path[78].Y.Should().Be(3);
            path[79].X.Should().Be(2);
            path[79].Y.Should().Be(2);
            path[80].X.Should().Be(1);
            path[80].Y.Should().Be(1);

        }
    }
}
