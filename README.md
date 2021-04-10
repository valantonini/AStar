A 2D A* (A Star) algorithm for C#
=====

![Travis (.com) branch](https://img.shields.io/travis/com/valantonini/AStar/master?style=for-the-badge)
[![NuGet](https://img.shields.io/nuget/v/AStarLite.svg?style=for-the-badge)](https://www.nuget.org/packages/AStarLite/)

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

## Example usage
<div style="text-align: center">
   <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" width="201px" height="201px" viewBox="-0.5 -0.5 201 201" content="&lt;mxfile host=&quot;www.draw.io&quot; modified=&quot;2021-04-10T00:55:32.463Z&quot; agent=&quot;5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0.3 Safari/605.1.15&quot; etag=&quot;fb0zhXeOA9UlfdDWiNH5&quot; version=&quot;14.5.10&quot; type=&quot;device&quot;&gt;&lt;diagram id=&quot;TbOAegxa6cETFJy4mpEc&quot; name=&quot;Page-1&quot;&gt;7Vnfb9owEP5reASRmNDwWChsUztpaidNfULGMYmHk4scp8D++tmJHUh/AJVAoQgeIPf5Yp/vu7tcTAuN4tU3gdPoJwSUt9xusGqhu5brDnp99a2BdQl4qFcCoWBBCTkb4In9owbsGjRnAc1qihKAS5bWQQJJQomsYVgIWNbV5sDrq6Y4pG+AJ4L5W/QPC2RUor57s8G/UxZGdmWnPyhHYmyVzU6yCAew3ILQuIVGAkCWV/FqRLn2nfVLed/kg9HKMEETecgN0+WgjR5n4/Hk9/Piefa0fszv2245ywvmudmwMVaurQdeqJBMOeQBzyj/BRmTDBI1NAMpIW6hYbWxrhICnEU0MALmLNSqRJlIhQIiGXMlO+rSTntrdCSkei4pYFF52ilnT7Ul8SrUAdaJgSzytFMIjGSdjMUppz+ItmmYLagkkVl9zjgfAQdR7AMFHvWDXrWGHUkgUdMPjR+UUXT1oYOdijYV7hRiKsVaqZgbkI1ZE+qOb+TlJnA8A0VbMWMxbEI1rGbesKkuDKGfIBddGLk13hSjM9/ree8wPfcJJeR1OByDYd87L4Z7F8Zwk+nb659Z+vYvjNyzS1/X85pl2DK6i2IaqH7EiCBkBCEkmI836FBAngQVrxudB9C0FUT9pVKuTXOFcwl1uss19UK7HavsglwQumNHvmnRsAip3KHndN9nSlCOJXupG3J0v/sXllnn1PUg1HDZHFzJPR65rypm4+S6B3Q8X6ti2vTZXzIHTZZMa+Y1rU7QajaeVs4BRdO6l8XFkcl+hvaGA9cDQ0wWYZGQ2y1g8VEqxWK3WVoe7RSRYoU5W+nwGRp77iIp9ZnQrfaEOyFB4naY4nrOVKqLDlErupMAS6x+NJ7pX8jaN20rae9OlDrLoumc47DtuH4nTcLXgbsrdCrDIcWESc0vOlL3enbvJ3YLl1OL3UNrsd9kKXYPKMVfLlnR3mQNKV3oI62J7gEmdxDD9D5PpiTCAhP10DhSuvaP9bJ5bqeBN9cH+Mke4I2fJHhXck/2RntCcpW4+QOoGNv6Fw2N/wM=&lt;/diagram&gt;&lt;/mxfile&gt;" style="background-color: rgb(255, 255, 255);"><defs/><g><rect x="0" y="0" width="50" height="50" fill="#d5e8d4" stroke="none" pointer-events="all"/><path d="M 0 0 L 50 50 M 0 50 L 50 0" fill="none" stroke="none" pointer-events="all"/><rect x="75" y="0" width="50" height="50" fill="#f8cecc" stroke="#b85450" pointer-events="all"/><path d="M 75 0 L 125 50 M 75 50 L 125 0" fill="none" stroke="#b85450" stroke-miterlimit="10" pointer-events="all"/><rect x="150" y="0" width="50" height="50" fill="#d5e8d4" stroke="none" pointer-events="all"/><path d="M 150 0 L 200 50 M 150 50 L 200 0" fill="none" stroke="none" pointer-events="all"/><rect x="75" y="75" width="50" height="50" fill="#f8cecc" stroke="#b85450" pointer-events="all"/><path d="M 75 75 L 125 125 M 75 125 L 125 75" fill="none" stroke="#b85450" stroke-miterlimit="10" pointer-events="all"/><path d="M 50 175 L 143.63 175" fill="none" stroke="#000000" stroke-miterlimit="10" pointer-events="stroke"/><path d="M 148.88 175 L 141.88 178.5 L 143.63 175 L 141.88 171.5 Z" fill="#000000" stroke="#000000" stroke-miterlimit="10" pointer-events="all"/><rect x="0" y="150" width="50" height="50" fill="#d5e8d4" stroke="none" pointer-events="all"/><path d="M 0 150 L 50 200 M 0 200 L 50 150" fill="none" stroke="none" pointer-events="all"/><rect x="75" y="150" width="50" height="50" fill="#d5e8d4" stroke="none" pointer-events="all"/><path d="M 75 150 L 125 200 M 75 200 L 125 150" fill="none" stroke="none" pointer-events="all"/><path d="M 175 150 L 175 56.37" fill="none" stroke="#000000" stroke-miterlimit="10" pointer-events="stroke"/><path d="M 175 51.12 L 178.5 58.12 L 175 56.37 L 171.5 58.12 Z" fill="#000000" stroke="#000000" stroke-miterlimit="10" pointer-events="all"/><rect x="150" y="150" width="50" height="50" fill="#d5e8d4" stroke="none" pointer-events="all"/><path d="M 150 150 L 200 200 M 150 200 L 200 150" fill="none" stroke="none" pointer-events="all"/><image x="149.5" y="-0.5" width="50" height="50" xlink:href="https://cdn2.iconfinder.com/data/icons/ios-7-icons/50/finish_flag-128.png" preserveAspectRatio="none" opacity="0.3"/><path d="M 25 50 L 25 143.63" fill="none" stroke="#000000" stroke-miterlimit="10" pointer-events="stroke"/><path d="M 25 148.88 L 21.5 141.88 L 25 143.63 L 28.5 141.88 Z" fill="#000000" stroke="#000000" stroke-miterlimit="10" pointer-events="all"/><image x="-0.5" y="-0.5" width="50" height="50" xlink:href="https://cdn3.iconfinder.com/data/icons/geek-3/24/Domo_Kun_character-128.png" preserveAspectRatio="none" opacity="0.6"/><rect x="150" y="75" width="50" height="50" fill="#d5e8d4" stroke="none" pointer-events="all"/><path d="M 150 75 L 200 125 M 150 125 L 200 75" fill="none" stroke="none" pointer-events="all"/><rect x="0" y="75" width="50" height="50" fill="#d5e8d4" stroke="none" pointer-events="all"/><path d="M 0 75 L 50 125 M 0 125 L 50 75" fill="none" stroke="none" pointer-events="all"/></g></svg>
</div>

```csharp
   var pathfinderOptions = new PathFinderOptions { 
      PunishChangeDirection = true,
      UseDiagonals = false, 
   };

   var tiles = new short[] {
      { 1, 0, 1 },
      { 1, 0, 1 },
      { 1, 1, 1 },
   };

    var worldGrid = new WorldGrid(tiles)
    var pathfinder = new PathFinder(worldGrids, pathfinderOptions);
    
    // The following are equivalent
    Position[] path = pathfinder.FindPath(new Position(0, 0), new Position(0, 2));
    Point[] path = pathfinder.FindPath(new Point(0, 0), new Point(2, 1));
```

## Options
 - Allowing / restricting diagonal movement
 - A choice of heuristic (Manhattan, MaxDxDy, Euclidean, Diagonal shortcut)
 - The option to punish direction changes.
 - A search limit to short circuit the search


## Changes from 0.1.x to 1.0.0
- The world is now represented by a WorldGrid that uses shorts internally instead of bytes
- If no path is found, the algorithm now reports an empty array instead of null
- Moved out of the AStar.Core namespace into simply AStar
- Replaced former Point class with the new Position class that uses Row / Column instead of X / Y to avoid confusion with cartesian co-ordinates
- Implemented support for Point class indexing and pathing which represent a traditional cartesian co-ordinate system
- Changed path from List to Array and changed type from PathFinderNode to Position or Point
- Reversed the order of the returned path to start at the start node
- Rationalised and dropped buggy options (heavy diagonals)
