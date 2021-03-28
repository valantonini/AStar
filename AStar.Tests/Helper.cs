using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStar.Tests
{
    public static class Helper
    {
        public static string PrintGrid(PathfinderGrid pathfinderGrid, bool appendSpace = true)
        {
            var s = new StringBuilder();

            for (var row = 0; row < pathfinderGrid.Height; row++)
            {
                for (var column = 0; column < pathfinderGrid.Width; column++)
                {
                    s.Append(pathfinderGrid[row, column]);
                    if (appendSpace)
                    {
                        s.Append(' ');
                    }
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }

        public static string PrintPath(PathfinderGrid pathfinderGrid, Position[] path, bool appendSpace = true)
        {
            var s = new StringBuilder();
            
            for (var row = 0; row < pathfinderGrid.Height; row++)
            {
                for (var column = 0; column < pathfinderGrid.Width; column++)
                {
                    if (path.Any(n => n.Row == row && n.Column == column))
                    {
                        s.Append("_");
                    }
                    else
                    {
                        s.Append(pathfinderGrid[row, column]);
                    }
                    s.Append(' ');
                }
                s.Append(Environment.NewLine);
            }
            return s.ToString();
        }

        public static void Print(PathfinderGrid pathfinderGrid, Position[] path)
        {
            Console.WriteLine(PrintGrid(pathfinderGrid));
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(PrintPath(pathfinderGrid, path));
        }

        public static void PrintAssertions(Position[] path)
        {
            for (var i = 0; i < path.Length; i++)
            {
                Console.WriteLine("path[{0}].Position.Row.ShouldBe({1});", i, path[i].Row);
                Console.WriteLine("path[{0}].Position.Column.ShouldBe({1});", i, path[i].Column);
            }
        }

        public static PathfinderGrid ConvertStringToPathfinderGrid(string level)
        {
            var closedCharacter = 'X';
            
            var splitLevel = level.Split('\n')
                .Select(row => row.Trim())
                .ToList();
            
            var pathfinderGrid = new PathfinderGrid(splitLevel.Count, splitLevel[0].Length);

            for (var row = 0; row < splitLevel.Count; row++)
            {
                for (var column = 0; column < splitLevel[row].Length; column++)
                {
                    if (splitLevel[row][column] != closedCharacter)
                    {
                        pathfinderGrid[row, column] = int.Parse(splitLevel[row][column].ToString());
                    }
                }
            }

            return pathfinderGrid;
        }
    }
}
