using System;
using System.Drawing;
using System.Linq;
using System.Text;
using Shouldly;

namespace AStar.Tests
{
    public static class Helper
    {
        public static string PrintGrid(WorldGrid worldGrid, bool appendSpace = true)
        {
            var s = new StringBuilder();

            for (var row = 0; row < worldGrid.Height; row++)
            {
                for (var column = 0; column < worldGrid.Width; column++)
                {
                    s.Append(worldGrid[row, column]);
                    if (appendSpace)
                    {
                        s.Append(' ');
                    }
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }

        public static string PrintPath(WorldGrid world, Position[] path, bool appendSpace = true)
        {
            var s = new StringBuilder();
            
            for (var row = 0; row < world.Height; row++)
            {
                for (var column = 0; column < world.Width; column++)
                {
                    if (path.Any(n => n.Row == row && n.Column == column))
                    {
                        s.Append("*");
                    }
                    else
                    {
                        s.Append(world[row, column]);
                    }
                    s.Append(' ');
                }
                s.Append(Environment.NewLine);
            }
            return s.ToString();
        }
        
        public static string PrintPath(WorldGrid world, Point[] path, bool appendSpace = true)
        {
            var s = new StringBuilder();
            
            for (var y = 0; y < world.Height; y++)
            {
                for (var x = 0; x < world.Width; x++)
                {
                    if (path.Any(n => n.Y == y && n.X == x))
                    {
                        s.Append("_");
                    }
                    else
                    {
                        s.Append(world[y, x]);
                    }
                    s.Append(' ');
                }
                s.Append(Environment.NewLine);
            }
            return s.ToString();
        }

        public static void Print(WorldGrid world, Position[] path)
        {
            Console.WriteLine(PrintGrid(world));
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(PrintPath(world, path));
            
            PrintAssertions(path);
        }
        
        public static void Print(WorldGrid world, Point[] path)
        {
            Print(world, path.Select(p => p.ToPosition()).ToArray());
        }

        public static void PrintAssertions(Position[] path)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("path.ShouldBe(new[] {");
            foreach (var position in path)
            {
                s.AppendLine($"new Position({position.Row}, {position.Column}),");
            }
            s.AppendLine("});");
            Console.WriteLine(s.ToString());
        }
        
        public static void PrintAssertions(Point[] path)
        {
            for (var i = 0; i < path.Length; i++)
            {
                Console.WriteLine("path[{0}].X.ShouldBe({1});", i, path[i].X);
                Console.WriteLine("path[{0}].Y.ShouldBe({1});", i, path[i].Y);
            }
        }

        public static WorldGrid ConvertStringToPathfinderGrid(string level)
        {
            var closedCharacter = 'X';
            
            var splitLevel = level.Split('\n')
                .Select(row => row.Trim())
                .ToList();
            
            var world = new WorldGrid(splitLevel.Count, splitLevel[0].Length);

            for (var row = 0; row < splitLevel.Count; row++)
            {
                for (var column = 0; column < splitLevel[row].Length; column++)
                {
                    if (splitLevel[row][column] != closedCharacter)
                    {
                        world[row, column] = short.Parse(splitLevel[row][column].ToString());
                    }
                }
            }

            return world;
        }
    }
}
