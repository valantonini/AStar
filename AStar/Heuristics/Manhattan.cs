using System;

namespace AStar.Heuristics
{
    public class Manhattan : ICalculateHeuristic
    {
        public int CalculateHeuristic(Position source, Position destination)
        {
            var heuristicEstimate = 2;
            var h = heuristicEstimate * (Math.Abs(source.Row - destination.Row) + Math.Abs(source.Column - destination.Column));
            return h;
        }
    }
}