using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStar.Tests
{
    public class Helper
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

        public static string PrintPath(PathfinderGrid pathfinderGrid, List<PathFinderNode> path, bool appendSpace = true)
        {
            var s = new StringBuilder();
            
            for (var row = 0; row < pathfinderGrid.Height; row++)
            {
                for (var column = 0; column < pathfinderGrid.Width; column++)
                {
                    if (path.Any(n => n.X == row && n.Y == column))
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

        public static void Print(PathfinderGrid pathfinderGrid, List<PathFinderNode> path)
        {
            Console.WriteLine(PrintGrid(pathfinderGrid));
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(PrintPath(pathfinderGrid, path));

            for (var i = 0; i < path.Count; i++)
            {
                Console.WriteLine("path[{0}].X.ShouldBe({1});", i, path[i].X);
                Console.WriteLine("path[{0}].Y.ShouldBe({1});", i, path[i].Y);
            }
        }

        public static void PrintAssertions(List<PathFinderNode> path)
        {
            for (var i = 0; i < path.Count; i++)
            {
                Console.WriteLine("path[{0}].X.ShouldBe({1});", i, path[i].X);
                Console.WriteLine("path[{0}].Y.ShouldBe({1});", i, path[i].Y);
            }
        }
    }
}
