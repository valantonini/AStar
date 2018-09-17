using System;

namespace AStar
{
    public class Point : IEquatable<Point>
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Point(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public static bool operator ==(Point a, Point b)
        {
            return a?.Row == b?.Row && a?.Column == b?.Column;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        public bool Equals(Point other)
        {
            return Row == other?.Row && Column == other?.Column;
        }

        public override bool Equals(Object other)
        {
            var point2 = other as Point;

            if (other == null)
            {
                return false;
            }

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return $"[{Row},{Column}]".GetHashCode();
        }
    }
}