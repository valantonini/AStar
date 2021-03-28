using AStar.Heuristics;

namespace AStar
{
    public class PathFinderOptions
    {
        public HeuristicFormula HeuristicFormula { get; set; }

        public DiagonalOptions DiagonalOptions { get; set; }

        public bool PunishChangeDirection { get; set; }

        public int SearchLimit { get; set; }

        internal bool UseDiagonals => DiagonalOptions == DiagonalOptions.Diagonals || DiagonalOptions == DiagonalOptions.HeavyDiagonals;

        public PathFinderOptions()
        {
            HeuristicFormula = HeuristicFormula.Manhattan;
            DiagonalOptions = DiagonalOptions.Diagonals;
            SearchLimit = 2000;
        }
    }

    public enum DiagonalOptions
    {
        NoDiagonals = 0,
        Diagonals = 1,
        HeavyDiagonals = 2
    }
}
