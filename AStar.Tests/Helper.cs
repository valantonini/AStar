using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStar.Tests
{
    public class Helper
    {
        public static string PrintGrid(byte[,] grid, bool appendSpace = true)
        {
            var s = new StringBuilder();

            for (var row = 0; row < grid.GetLength(0); row++)
            {
                for (var column = 0; column < grid.GetLength(1); column++)
                {
                    s.Append(grid[row, column]);
                    if (appendSpace) s.Append(' ');
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }

        public static string PrintPath(byte[,] grid, List<PathFinderNode> path, bool appendSpace = true)
        {
            var s = new StringBuilder();

            for (var row = 0; row < grid.GetLength(0); row++)
            {
                for (var column = 0; column < grid.GetLength(1); column++)
                {
                    if (path.Any(n => n.X == row && n.Y == column))
                    {
                        s.Append("X");
                    }
                    else
                    {
                        s.Append(grid[row, column]);
                    }
                    s.Append(' ');
                }
                s.Append(Environment.NewLine);
            }
            return s.ToString();
        }

        public static void Print(byte[,] grid, List<PathFinderNode> path)
        {
            Console.WriteLine(PrintGrid(grid));
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(PrintPath(grid, path));

            for (var i = 0; i < path.Count; i++)
            {
                Console.WriteLine("path[{0}].X.ShouldBe({1});", i, path[i].X);
                Console.WriteLine("path[{0}].Y.ShouldBe({1});", i, path[i].Y);
            }
        }
    }
}
