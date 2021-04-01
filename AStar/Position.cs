using System;

namespace AStar
{
    /// <summary>
    /// A point in a matrix. P(row, column)
    /// </summary>
    public readonly struct Position
    {
        /// <summary>
        /// The row in the matrix
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// The column in the matrix
        /// </summary>
        public int Column { get; }

        public Position(int row = 0, int column = 0)
        {
            Row = row;
            Column = column;
        }

        public bool IsDiagonalTo(Position other)
        {
            // return Row - other.Row != 0 ||
            //     Column - other.Column != 0;
            
            return Row != other.Row &&
                Column != other.Column;
        }
        
        public static bool operator ==(Position a, Position b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Position a, Position b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(Object other)
        {
            if (other is Position otherPoint)
            {
                return Row == otherPoint.Row && Column == otherPoint.Column;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Row.GetHashCode();
                hash = hash * 23 + Column.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"[{Row}.{Column}]";
        }
    }
}