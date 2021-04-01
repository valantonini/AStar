using System;

namespace AStar.Heuristics
{
    public class MaxDXDY : ICalculateHeuristic
    {
        public int Calculate(Position source, Position destination)
        {
            var heuristicEstimate = 2;
            var h = heuristicEstimate * (Math.Max(Math.Abs(source.Row - destination.Row), Math.Abs(source.Column - destination.Column)));
            return h;
        }
    }
}