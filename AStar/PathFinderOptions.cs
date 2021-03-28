using AStar.Heuristics;

namespace AStar
{
    public class PathFinderOptions
    {
        public HeuristicFormula HeuristicFormula { get; set; }

        public bool Diagonals { get; set; }

        public bool HeavyDiagonals { get; set; }

        public bool PunishChangeDirection { get; set; }

        public int SearchLimit { get; set; }

        public PathFinderOptions()
        {
            HeuristicFormula = HeuristicFormula.Manhattan;
            SearchLimit = 2000;
            Diagonals = true;
        }
    }
}
