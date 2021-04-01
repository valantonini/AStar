using System;

namespace AStar.Heuristics
{
    public static class HeuristicFactory
    {
        public static ICalculateHeuristic Create(HeuristicFormula heuristicFormula)
        {
            switch (heuristicFormula)
            {
                case HeuristicFormula.Manhattan:
                    return new Manhattan();
                case HeuristicFormula.MaxDXDY:
                    return new MaxDXDY();
                case HeuristicFormula.DiagonalShortCut:
                    return new DiagonalShortcut();
                case HeuristicFormula.Euclidean:
                    return new Euclidean();
                case HeuristicFormula.EuclideanNoSQR:
                    return new EuclideanNoSQR();
                case HeuristicFormula.Custom1:
                    return new Custom1();
                default:
                    throw new ArgumentOutOfRangeException(nameof(heuristicFormula), heuristicFormula, null);
            }
        }
    }
}