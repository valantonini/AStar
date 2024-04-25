using AStar.Heuristics;

namespace AStar.Options
{
    public class PathFinderOptions
    {
        public HeuristicFormula HeuristicFormula { get; set; }

        public bool UseDiagonals { get; set; }

        public bool PunishChangeDirection { get; set; }

        public int SearchLimit { get; set; }

        public Weighting Weighting {get;set;}

        public PathFinderOptions()
        {
            HeuristicFormula = HeuristicFormula.Manhattan;
            UseDiagonals = true;
            SearchLimit = 2000;
        }
    }

    public enum Weighting {
        // The number in the grid will not influence the path.
        None,
        // Higher open values will be favoured and applied to the new h value.
        Positive,
        // Lower open values will be favoured and applied to the new h value.
        Negative
    }
}
