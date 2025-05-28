using System;
using Xunit;
using BattleShip.Enginev2;

namespace BattleShip_Tests
{
    public class GameEngineTests
    {
        [Fact]
        public void PlaceShips_ShouldPlaceExactNumberOfShips()
        {
            var engine = new GameEngine(gridSize: 5, seed: 0);
            engine.PlaceShips(3);
            int count = 0;
            foreach (var s in engine.Ships) if (s) count++;
            Assert.Equal(3, count);
            Assert.Equal(3, engine.ShipsLeft);
        }

        [Fact]
        public void Shoot_MissDoesNotDecrementShipsLeft()
        {
            var engine = new GameEngine(5, seed: 0);
            engine.PlaceShips(1);
            bool hit = engine.Shoot(0, 0); 
            Assert.False(hit);
            Assert.Equal(1, engine.ShipsLeft);
            Assert.Equal(1, engine.ShotsCount);
        }

        [Fact]
        public void Shoot_OutOfRange_Throws()
        {
            var engine = new GameEngine();
            engine.PlaceShips(1);
            Assert.Throws<IndexOutOfRangeException>(() => engine.Shoot(10, 10));
        }

        [Fact]
        public void Reset_ShouldClearShotsAndPlaceShipsAgain()
        {
            var engine = new GameEngine(5, seed: 0);
            engine.PlaceShips(2);
            engine.Shoot(1, 1);
            Assert.Equal(1, engine.ShotsCount);
            engine.StartNewGame(2);
            Assert.Equal(0, engine.ShotsCount);
            int count = 0;
            foreach (var s in engine.Ships) if (s) count++;
            Assert.Equal(2, count);
        }
    }
}