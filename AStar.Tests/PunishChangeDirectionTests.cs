using AStar.Collections;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class PunishChangeDirectionTests
    {
        private WorldGrid _world;

        [SetUp]
        public void SetUp()
        {
            var level = @"  111111111X
                            111111111X
                            11111X111X
                            11111X111X
                            11111X111X
                            111111111X
                            111111111X
                            111111111X
                            111XXX111X
                            111111111X
                            111X1X111X
                            111111111X
                            111111111X
                            1111XX111X
                            11XXXX111X
                            1111XXX11X
                            1111XXX11X
                            1111XXX11X
                            111111111X
                            111111111X";

            _world = Helper.ConvertStringToPathfinderGrid(level);
        }
        
        [Test]
        public void ShouldPathPredictably3()
        {
            var pathfinder = new PathFinder(_world, new PathFinderOptions { DiagonalOptions = DiagonalOptions.Diagonals, PunishChangeDirection = true});

            var path = pathfinder.FindPath(new Position(2, 9), new Position(15, 3));

            Helper.Print(_world, path);
            
            path[0].Row.ShouldBe(15);
            path[0].Column.ShouldBe(3);
            path[1].Row.ShouldBe(15);
            path[1].Column.ShouldBe(2);
            path[2].Row.ShouldBe(14);
            path[2].Column.ShouldBe(1);
            path[3].Row.ShouldBe(13);
            path[3].Column.ShouldBe(2);
            path[4].Row.ShouldBe(12);
            path[4].Column.ShouldBe(3);
            path[5].Row.ShouldBe(12);
            path[5].Column.ShouldBe(4);
            path[6].Row.ShouldBe(12);
            path[6].Column.ShouldBe(5);
            path[7].Row.ShouldBe(11);
            path[7].Column.ShouldBe(6);
            path[8].Row.ShouldBe(10);
            path[8].Column.ShouldBe(6);
            path[9].Row.ShouldBe(9);
            path[9].Column.ShouldBe(6);
            path[10].Row.ShouldBe(8);
            path[10].Column.ShouldBe(6);
            path[11].Row.ShouldBe(7);
            path[11].Column.ShouldBe(6);
            path[12].Row.ShouldBe(6);
            path[12].Column.ShouldBe(6);
            path[13].Row.ShouldBe(5);
            path[13].Column.ShouldBe(6);
            path[14].Row.ShouldBe(4);
            path[14].Column.ShouldBe(6);
            path[15].Row.ShouldBe(3);
            path[15].Column.ShouldBe(6);
            path[16].Row.ShouldBe(2);
            path[16].Column.ShouldBe(7);
            path[17].Row.ShouldBe(2);
            path[17].Column.ShouldBe(8);
            path[18].Row.ShouldBe(2);
            path[18].Column.ShouldBe(9);
            
            
        }
        
    }
}