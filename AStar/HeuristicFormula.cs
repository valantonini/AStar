using System;

namespace AStar
{
    public class Heuristic
    {
        public static int DetermineH(HeuristicFormula heuristicFormula, Position end, int heuristicEstimate, int newLocationY, int newLocationX)
        {
            int h;

            switch (heuristicFormula)
            {
                case HeuristicFormula.MaxDXDY:
                    h = heuristicEstimate * (Math.Max(Math.Abs(newLocationX - end.Row), Math.Abs(newLocationY - end.Column)));
                    break;

                case HeuristicFormula.DiagonalShortCut:
                    var hDiagonal = Math.Min(Math.Abs(newLocationX - end.Row),
                        Math.Abs(newLocationY - end.Column));
                    var hStraight = (Math.Abs(newLocationX - end.Row) + Math.Abs(newLocationY - end.Column));
                    h = (heuristicEstimate * 2) * hDiagonal + heuristicEstimate * (hStraight - 2 * hDiagonal);
                    break;

                case HeuristicFormula.Euclidean:
                    h = (int)(heuristicEstimate * Math.Sqrt(Math.Pow((newLocationY - end.Row), 2) + Math.Pow((newLocationY - end.Column), 2)));
                    break;

                case HeuristicFormula.EuclideanNoSQR:
                    h = (int)(heuristicEstimate * (Math.Pow((newLocationX - end.Row), 2) + Math.Pow((newLocationY - end.Column), 2)));
                    break;

                case HeuristicFormula.Custom1:
                    var dxy = new Position(Math.Abs(end.Row - newLocationX), Math.Abs(end.Column - newLocationY));
                    var Orthogonal = Math.Abs(dxy.Row - dxy.Column);
                    var Diagonal = Math.Abs(((dxy.Row + dxy.Column) - Orthogonal) / 2);
                    h = heuristicEstimate * (Diagonal + Orthogonal + dxy.Row + dxy.Column);
                    break;

                // ReSharper disable once RedundantCaseLabel
                case HeuristicFormula.Manhattan:
                default:
                    h = heuristicEstimate * (Math.Abs(newLocationX - end.Row) + Math.Abs(newLocationY - end.Column));
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
