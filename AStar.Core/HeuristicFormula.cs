using System;

namespace AStar
{
    public class Heuristic
    {
        public static int DetermineH(HeuristicFormula heuristicFormula, Point end, int heuristicEstimate, int newLocationY, int newLocationX)
        {
            int h;

            switch (heuristicFormula)
            {
                case HeuristicFormula.MaxDXDY:
                    h = heuristicEstimate * (Math.Max(Math.Abs(newLocationX - end.X), Math.Abs(newLocationY - end.Y)));
                    break;

                case HeuristicFormula.DiagonalShortCut:
                    var hDiagonal = Math.Min(Math.Abs(newLocationX - end.X),
                        Math.Abs(newLocationY - end.Y));
                    var hStraight = (Math.Abs(newLocationX - end.X) + Math.Abs(newLocationY - end.Y));
                    h = (heuristicEstimate * 2) * hDiagonal + heuristicEstimate * (hStraight - 2 * hDiagonal);
                    break;

                case HeuristicFormula.Euclidean:
                    h = (int)(heuristicEstimate * Math.Sqrt(Math.Pow((newLocationY - end.X), 2) + Math.Pow((newLocationY - end.Y), 2)));
                    break;

                case HeuristicFormula.EuclideanNoSQR:
                    h = (int)(heuristicEstimate * (Math.Pow((newLocationX - end.X), 2) + Math.Pow((newLocationY - end.Y), 2)));
                    break;

                case HeuristicFormula.Custom1:
                    var dxy = new Point(Math.Abs(end.X - newLocationX), Math.Abs(end.Y - newLocationY));
                    var Orthogonal = Math.Abs(dxy.X - dxy.Y);
                    var Diagonal = Math.Abs(((dxy.X + dxy.Y) - Orthogonal) / 2);
                    h = heuristicEstimate * (Diagonal + Orthogonal + dxy.X + dxy.Y);
                    break;

                // ReSharper disable once RedundantCaseLabel
                case HeuristicFormula.Manhattan:
                default:
                    h = heuristicEstimate * (Math.Abs(newLocationX - end.X) + Math.Abs(newLocationY - end.Y));
                    break;
            }

            return h;
        }
    }

    public enum HeuristicFormula
    {
        // ReSharper disable InconsistentNaming
        Manhattan = 1,
        MaxDXDY = 2,
        DiagonalShortCut = 3,
        Euclidean = 4,
        EuclideanNoSQR = 5,
        Custom1 = 6
        // ReSharper restore InconsistentNaming
    }
}
