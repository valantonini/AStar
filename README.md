AStar
=====

[![Build Status](https://travis-ci.org/valantonini/AStar.svg?branch=master)](https://travis-ci.org/valantonini/AStar)

An A Star (A*) algorithm for C# based on [Gustavo Franco's implementation](http://www.codeguru.com/csharp/csharp/cs_misc/designtechniques/article.php/c12527/AStar-A-Implementation-in-C-Path-Finding-PathFinder.htm).

The grid is represented as a 2d byte array (byte[,]). Blocked locations are marked as a 0, anything else is considered traversable.

Unlike a cartesian coordinate system that uses X for horizontal and Y for vertical on a 2D plane, the point used in this library represents a matrix index where X is the row and Y is the column (P<sub>xy</sub>)

Options allow for the use of diagonals and punishing direction changes.

    var pathfinderOptions = new PathFinderOptions {PunishChangeDirection = true};

    var pathfinder = new PathFinder(new byte[40,40], pathfinderOptions);
    
    var path = pathfinder.FindPath(new Point(1, 1), new Point(30, 30));