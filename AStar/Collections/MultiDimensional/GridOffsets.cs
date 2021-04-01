using System.Collections.Generic;
using System.Linq;

namespace AStar.Collections.MultiDimensional
{
    public static class GridOffsets
    {
        private static IEnumerable<(sbyte row, sbyte column)> CardinalDirectionOffsets
        {
            get
            {
                yield return (0, -1);
                yield return (1, 0);
                yield return (0, 1);
                yield return (-1, 0);
            }
        }

        private static IEnumerable<(sbyte row, sbyte column)> DiagonalsOffsets
        {
            get
            {
                yield return (1, -1);
                yield return (1, 1);
                yield return (-1, 1);
                yield return (-1, -1);
            }
        }

        public static IEnumerable<(sbyte row, sbyte column)> GetOffsets(bool withDiagonals = false)
        {
            return withDiagonals 
                ? CardinalDirectionOffsets.Concat(DiagonalsOffsets) 
                : CardinalDirectionOffsets;
        }

        public static bool IsCardinalOffset((sbyte row, sbyte column) offset)
        {
            return offset.row != 0 && offset.column != 0;
        }

        public static bool IsDiagonal((sbyte row, sbyte column) offset)
        {
            return offset.row != 0 || offset.column != 0;
        }
    }
}