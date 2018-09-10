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

            for (var x = 0; x < grid.GetUpperBound(0); x++)
            {
                for (var y = 0; y < grid.GetUpperBound(1); y++)
                {
                    s.Append(grid[x, y]);
                    if (appendSpace) s.Append(' ');
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }

        public static string PrintPath(byte[,] grid, List<PathFinderNode> path, bool appendSpace = true)
        {
            var s = new StringBuilder();

            for (var x = 0; x < grid.GetUpperBound(0); x++)
            {
                for (var y = 0; y < grid.GetUpperBound(1); y++)
                {
                    if (path.Any(n => n.X == x && n.Y == y))
                    {
                        s.Append("X");
                    }
                    else
                    {
                        s.Append(grid[x, y]);
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
