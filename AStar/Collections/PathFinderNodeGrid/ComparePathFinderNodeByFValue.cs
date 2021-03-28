using System.Collections.Generic;

namespace AStar.Collections.PathFinderNodeGrid
{
    internal class ComparePathFinderNodeByFValue : IComparer<Position>
    {
        readonly CalculationGrid _world;

        public ComparePathFinderNodeByFValue(CalculationGrid world)
        {
            _world = world;
        }

        public int Compare(Position a, Position b)
        {
            if (_world[a].F > _world[b].F)
            {
                return 1;
            }

            if (_world[a].F < _world[b].F)
            {
                return -1;
            }
            
            return 0;
        }
    }
}