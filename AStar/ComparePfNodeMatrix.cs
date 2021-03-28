using System.Collections.Generic;
using AStar.Collections;

namespace AStar
{
    internal class ComparePfNodeMatrix : IComparer<Position>
    {
        readonly CalculationGrid _matrix;

        public ComparePfNodeMatrix(CalculationGrid matrix)
        {
            _matrix = matrix;
        }

        public int Compare(Position a, Position b)
        {
            if (_matrix[a].F > _matrix[b].F)
            {
                return 1;
            }

            if (_matrix[a].F < _matrix[b].F)
            {
                return -1;
            }
            
            return 0;
        }
    }
}