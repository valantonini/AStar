A Star (A*) algorithm for C#.
=====

[![Build Status](https://travis-ci.org/valantonini/AStar.svg?branch=development)](https://travis-ci.org/valantonini/AStar)
[![NuGet](https://img.shields.io/nuget/v/AStarLite.svg)](https://www.nuget.org/packages/AStarLite/)


The world is represented by a WorldMatrix that is essentially a matrix of the C# short data type.
A value of 0 indicates the cell is closed / blocked. Any other number indicates the cell is open and traversable.

The world grid can be indexed via either:

1) The position struct where a row is the vertical axis and column is the horizontal axis 
   similar to indexing into a matrix (P<sub>rc</sub>).
   
2) The build in Point struct that operates like a cartesian co-ordinate system where 
   x is the horizontal axis and y is the vertical axis (P<sub>xy</sub>).

Paths can be found using either Positions (matrix indexing) or Points (cartesian indexing).

```csharp
    var pathfinderOptions = new PathFinderOptions { PunishChangeDirection = true };

    var pathfinder = new PathFinder(new WorldGrid(40,40), pathfinderOptions);
    
    Position[] path = pathfinder.FindPath(new Position(1, 1), new Position(30, 30));
    Point[] path = pathfinder.FindPath(new Point(1, 1), new Point(30, 30));
```

Options include:
 - allowing / restricting diagonal movement
 - choice of heuristic (Manhattan, MaxDxDy, Euclidean, Diagonal shortcut)
 - option to punish direction changes.

## Example usage
```csharp
    var pathfinderOptions = new PathFinderOptions { PunishChangeDirection = true };

    var pathfinder = new PathFinder(new WorldGrid(40,40), pathfinderOptions);
    
    var path = pathfinder.FindPath(new Position(1, 1), new Position(30, 30));
```

## Changes from 0.1.x to 0.2.x
- The world is now represented by a WorldGrid and that uses shorts internally instead of bytes
- If no path is found, the algorithm now reports an empty array instead of null
- Moved out of the AStar.Core namespace into simply AStar
- Replaced former Point class with Position class that uses Row / Column instead of X / Y to avoid confusion with cartesian co-ordinates
- Implemented support for Point class indexing and pathing which represent a traditional cartesian co-ordinate system
- Changed path from List to Array and changed type from PathFinderNode to Position or Point
- Reversed the order of the returned path to start at the start node
- Rationalised and dropped buggy options (heavy diagonals)
