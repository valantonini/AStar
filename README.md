A Star (A*) algorithm for C#.
=====

![Travis (.com) branch](https://img.shields.io/travis/com/valantonini/AStar/master?style=for-the-badge)
[![NuGet](https://img.shields.io/nuget/v/AStarLite.svg?style=for-the-badge)](https://www.nuget.org/packages/AStarLite/)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/AStarLite?style=for-the-badge)](https://www.nuget.org/packages/AStarLite/)

The world is represented by a WorldGrid that is essentially a matrix of the C# short data type.
A value of 0 indicates the cell is closed / blocked. Any other number indicates the cell is open and traversable.
It is recommended to use 1 for open cells as numbers greater and less than 0 may be used to apply penalty or
priority to movement through those nodes in the future.

The WorldGrid can be indexed via either:

1) The provided Position struct where a row represents the vertical axis and column the horizontal axis 
   (Similar to indexing into a matrix P<sub>rc</sub>).
   
2) The C# Point struct that operates like a cartesian co-ordinate system where 
   X represents the horizontal axis and Y represents the vertical axis (P<sub>xy</sub>).

Paths can be found using either Positions (matrix indexing) or Points (cartesian indexing).

```csharp
    var pathfinderOptions = new PathFinderOptions { PunishChangeDirection = true };

    var pathfinder = new PathFinder(new WorldGrid(40,40), pathfinderOptions);
    
    // The following are equivalent
    Position[] path = pathfinder.FindPath(new Position(4, 1), new Position(28, 30));
    Point[] path = pathfinder.FindPath(new Point(1, 4), new Point(30, 28));
```

Options include:
 - Allowing / restricting diagonal movement
 - A choice of heuristic (Manhattan, MaxDxDy, Euclidean, Diagonal shortcut)
 - The option to punish direction changes.
 - A search limit to short circuit the search

## Example usage
```csharp
    var pathfinderOptions = new PathFinderOptions { PunishChangeDirection = true };

    var pathfinder = new PathFinder(new WorldGrid(40,40), pathfinderOptions);
    
    var path = pathfinder.FindPath(new Position(1, 1), new Position(30, 30));
```

## Changes from 0.1.x to 1.0.0
- The world is now represented by a WorldGrid that uses shorts internally instead of bytes
- If no path is found, the algorithm now reports an empty array instead of null
- Moved out of the AStar.Core namespace into simply AStar
- Replaced former Point class with the new Position class that uses Row / Column instead of X / Y to avoid confusion with cartesian co-ordinates
- Implemented support for Point class indexing and pathing which represent a traditional cartesian co-ordinate system
- Changed path from List to Array and changed type from PathFinderNode to Position or Point
- Reversed the order of the returned path to start at the start node
- Rationalised and dropped buggy options (heavy diagonals)
