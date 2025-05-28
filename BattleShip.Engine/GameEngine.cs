using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class GameEngine
    {
        public int GridSize { get; }
        public bool[,] Ships { get; private set; }
        public int ShipsLeft => _shipsLeft;
        public int ShotsCount => _shots;
        public int ElapsedSeconds => _seconds;

        private int _shipsLeft;
        private int _shots;
        private int _seconds;
        private readonly Random _rnd;

        public GameEngine(int gridSize = 5, int? seed = null)
        {
            GridSize = gridSize;
            Ships = new bool[gridSize, gridSize];
            _rnd = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        public void StartNewGame(int shipCount)
        {
            _shots = 0;
            _seconds = 0;
            PlaceShips(shipCount);
        }

        public void TickSecond()
            => _seconds++;

        public bool Shoot(int row, int col)
        {
            _shots++;
            if (Ships[row, col])
            {
                Ships[row, col] = false;
                _shipsLeft--;
                return true;
            }
            return false;
        }

        public void PlaceShips(int count)
        {
            Array.Clear(Ships, 0, Ships.Length);
            _shipsLeft = count;
            int placed = 0;
            while (placed < count)
            {
                int r = _rnd.Next(GridSize), c = _rnd.Next(GridSize);
                if (!Ships[r, c])
                {
                    Ships[r, c] = true;
                    placed++;
                }
            }
        }
    }
}
