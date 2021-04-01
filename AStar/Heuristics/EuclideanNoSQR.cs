using System;

namespace AStar.Heuristics
{
    public class EuclideanNoSQR : ICalculateHeuristic
    {
        public int Calculate(Position source, Position destination)
        {
            var heuristicEstimate = 2;
            var h = (int)(heuristicEstimate * (Math.Pow((source.Row - destination.Row), 2) + Math.Pow((source.Column - destination.Column), 2)));
            return h;
        }
    }
}