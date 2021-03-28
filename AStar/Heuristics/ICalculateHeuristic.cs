namespace AStar.Heuristics
{
    public interface ICalculateHeuristic
    {
        int CalculateHeuristic(Position source, Position destination);
    }
}