using System.Collections;
using System.Collections.Generic;

namespace AStar
{
    public static class GridOffsets
    {
        public static IEnumerable<(sbyte row, sbyte column)> CardinalDirectionOffsets
        {
            get
            {
                yield return (0, -1);
                yield return (1, 0);
                yield return (0, 1);
                yield return (-1, 0);
            }
        }

        public static IEnumerable<(sbyte row, sbyte column)> CardinalDirectionsWithDiagonalsOffsets
        {
            get
            {
                yield return (0, -1);
                yield return (1, 0);
                yield return (0, 1);
                yield return (-1, 0);
                yield return (1, -1);
                yield return (1, 1);
                yield return (-1, 1);
                yield return (-1, -1);
            }
        }

        public static IEnumerable<(sbyte row, sbyte column)> GetOffsets(bool withDiagonals = false)
        {
            return withDiagonals ? CardinalDirectionsWithDiagonalsOffsets : CardinalDirectionOffsets;
        }
    }
}