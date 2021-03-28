using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AStar.Collections;

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
                        s.Append("_");
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

        public static void Print(WorldGrid world, Position[] path)
        {
            Console.WriteLine(PrintGrid(world));
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(PrintPath(world, path));
        }

        public static void PrintAssertions(Position[] path)
        {
            for (var i = 0; i < path.Length; i++)
            {
                Console.WriteLine("path[{0}].Position.Row.ShouldBe({1});", i, path[i].Row);
                Console.WriteLine("path[{0}].Position.Column.ShouldBe({1});", i, path[i].Column);
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
                        world[row, column] = int.Parse(splitLevel[row][column].ToString());
                    }
                }
            }

            return world;
        }
    }
}
