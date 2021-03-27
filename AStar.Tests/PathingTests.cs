using System;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class PathingTests
    {
        private const char _closedCharacter = 'X';
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

            _pathfinderGrid = new PathfinderGrid(32, 32);
            var splitLevel = level.Split('\n')
                                  .Select(row => row.Trim())
                                  .ToList();

            for (var x = 0; x < splitLevel.Count; x++)
            {
                for (var y = 0; y < splitLevel[x].Length; y++)
                {
                    if (splitLevel[x][y] != _closedCharacter)
                    {
                        _pathfinderGrid[x, y] = Convert.ToByte(splitLevel[x][y]);
                    }

                }
            }

        }

        [Test]
        public void TestPathingOptions()
        {
            var pathfinderOptions = new PathFinderOptions { PunishChangeDirection = true };

            var pathfinder = new PathFinder(_pathfinderGrid, pathfinderOptions);
            var path = pathfinder.FindPath(new Point(1, 1), new Point(30, 30));
            Helper.Print(_pathfinderGrid, path);
        }

        [Test]
        public void ShouldPathEnvironment()
        {
            var pathfinder = new PathFinder(_pathfinderGrid);
            var path = pathfinder.FindPath(new Point(1, 1), new Point(30, 30));
            Helper.Print(_pathfinderGrid, path);

            path[0].X.ShouldBe(30);
            path[0].Y.ShouldBe(30);
            path[1].X.ShouldBe(30);
            path[1].Y.ShouldBe(29);
            path[2].X.ShouldBe(30);
            path[2].Y.ShouldBe(28);
            path[3].X.ShouldBe(30);
            path[3].Y.ShouldBe(27);
            path[4].X.ShouldBe(30);
            path[4].Y.ShouldBe(26);
            path[5].X.ShouldBe(30);
            path[5].Y.ShouldBe(25);
            path[6].X.ShouldBe(30);
            path[6].Y.ShouldBe(24);
            path[7].X.ShouldBe(30);
            path[7].Y.ShouldBe(23);
            path[8].X.ShouldBe(30);
            path[8].Y.ShouldBe(22);
            path[9].X.ShouldBe(30);
            path[9].Y.ShouldBe(21);
            path[10].X.ShouldBe(30);
            path[10].Y.ShouldBe(20);
            path[11].X.ShouldBe(30);
            path[11].Y.ShouldBe(19);
            path[12].X.ShouldBe(30);
            path[12].Y.ShouldBe(18);
            path[13].X.ShouldBe(30);
            path[13].Y.ShouldBe(17);
            path[14].X.ShouldBe(30);
            path[14].Y.ShouldBe(16);
            path[15].X.ShouldBe(30);
            path[15].Y.ShouldBe(15);
            path[16].X.ShouldBe(30);
            path[16].Y.ShouldBe(14);
            path[17].X.ShouldBe(30);
            path[17].Y.ShouldBe(13);
            path[18].X.ShouldBe(30);
            path[18].Y.ShouldBe(12);
            path[19].X.ShouldBe(30);
            path[19].Y.ShouldBe(11);
            path[20].X.ShouldBe(30);
            path[20].Y.ShouldBe(10);
            path[21].X.ShouldBe(30);
            path[21].Y.ShouldBe(9);
            path[22].X.ShouldBe(30);
            path[22].Y.ShouldBe(8);
            path[23].X.ShouldBe(30);
            path[23].Y.ShouldBe(7);
            path[24].X.ShouldBe(29);
            path[24].Y.ShouldBe(6);
            path[25].X.ShouldBe(28);
            path[25].Y.ShouldBe(6);
            path[26].X.ShouldBe(27);
            path[26].Y.ShouldBe(7);
            path[27].X.ShouldBe(27);
            path[27].Y.ShouldBe(8);
            path[28].X.ShouldBe(27);
            path[28].Y.ShouldBe(9);
            path[29].X.ShouldBe(27);
            path[29].Y.ShouldBe(10);
            path[30].X.ShouldBe(27);
            path[30].Y.ShouldBe(11);
            path[31].X.ShouldBe(27);
            path[31].Y.ShouldBe(12);
            path[32].X.ShouldBe(27);
            path[32].Y.ShouldBe(13);
            path[33].X.ShouldBe(27);
            path[33].Y.ShouldBe(14);
            path[34].X.ShouldBe(27);
            path[34].Y.ShouldBe(15);
            path[35].X.ShouldBe(27);
            path[35].Y.ShouldBe(16);
            path[36].X.ShouldBe(27);
            path[36].Y.ShouldBe(17);
            path[37].X.ShouldBe(27);
            path[37].Y.ShouldBe(18);
            path[38].X.ShouldBe(27);
            path[38].Y.ShouldBe(19);
            path[39].X.ShouldBe(27);
            path[39].Y.ShouldBe(20);
            path[40].X.ShouldBe(26);
            path[40].Y.ShouldBe(21);
            path[41].X.ShouldBe(25);
            path[41].Y.ShouldBe(22);
            path[42].X.ShouldBe(24);
            path[42].Y.ShouldBe(23);
            path[43].X.ShouldBe(23);
            path[43].Y.ShouldBe(24);
            path[44].X.ShouldBe(22);
            path[44].Y.ShouldBe(25);
            path[45].X.ShouldBe(21);
            path[45].Y.ShouldBe(26);
            path[46].X.ShouldBe(20);
            path[46].Y.ShouldBe(27);
            path[47].X.ShouldBe(19);
            path[47].Y.ShouldBe(28);
            path[48].X.ShouldBe(18);
            path[48].Y.ShouldBe(29);
            path[49].X.ShouldBe(17);
            path[49].Y.ShouldBe(28);
            path[50].X.ShouldBe(16);
            path[50].Y.ShouldBe(27);
            path[51].X.ShouldBe(15);
            path[51].Y.ShouldBe(26);
            path[52].X.ShouldBe(14);
            path[52].Y.ShouldBe(25);
            path[53].X.ShouldBe(13);
            path[53].Y.ShouldBe(24);
            path[54].X.ShouldBe(12);
            path[54].Y.ShouldBe(23);
            path[55].X.ShouldBe(11);
            path[55].Y.ShouldBe(22);
            path[56].X.ShouldBe(10);
            path[56].Y.ShouldBe(21);
            path[57].X.ShouldBe(10);
            path[57].Y.ShouldBe(20);
            path[58].X.ShouldBe(10);
            path[58].Y.ShouldBe(19);
            path[59].X.ShouldBe(10);
            path[59].Y.ShouldBe(18);
            path[60].X.ShouldBe(10);
            path[60].Y.ShouldBe(17);
            path[61].X.ShouldBe(10);
            path[61].Y.ShouldBe(16);
            path[62].X.ShouldBe(10);
            path[62].Y.ShouldBe(15);
            path[63].X.ShouldBe(10);
            path[63].Y.ShouldBe(14);
            path[64].X.ShouldBe(10);
            path[64].Y.ShouldBe(13);
            path[65].X.ShouldBe(10);
            path[65].Y.ShouldBe(12);
            path[66].X.ShouldBe(10);
            path[66].Y.ShouldBe(11);
            path[67].X.ShouldBe(10);
            path[67].Y.ShouldBe(10);
            path[68].X.ShouldBe(10);
            path[68].Y.ShouldBe(9);
            path[69].X.ShouldBe(10);
            path[69].Y.ShouldBe(8);
            path[70].X.ShouldBe(10);
            path[70].Y.ShouldBe(7);
            path[71].X.ShouldBe(10);
            path[71].Y.ShouldBe(6);
            path[72].X.ShouldBe(9);
            path[72].Y.ShouldBe(5);
            path[73].X.ShouldBe(8);
            path[73].Y.ShouldBe(4);
            path[74].X.ShouldBe(7);
            path[74].Y.ShouldBe(3);
            path[75].X.ShouldBe(6);
            path[75].Y.ShouldBe(3);
            path[76].X.ShouldBe(5);
            path[76].Y.ShouldBe(3);
            path[77].X.ShouldBe(4);
            path[77].Y.ShouldBe(3);
            path[78].X.ShouldBe(3);
            path[78].Y.ShouldBe(3);
            path[79].X.ShouldBe(2);
            path[79].Y.ShouldBe(2);
            path[80].X.ShouldBe(1);
            path[80].Y.ShouldBe(1);
        }
    }
}
