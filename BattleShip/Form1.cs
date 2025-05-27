using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class Form1 : Form
    {
        private const int GridSize = 5;
        private const int CellSize = 60;                 

        private readonly Button[,] _cells = new Button[GridSize, GridSize];
        private readonly bool[,] _ships = new bool[GridSize, GridSize];
        private int _shipsLeft;

        private int _shots;
        private Label lblShots;

        private int _seconds;
        private Label lblTime;
        private readonly Timer _timer = new Timer { Interval = 1000 }; // 1 с

        public Form1()
        {
            Text = "Battleship Light (timer)";
            ClientSize = new Size(CellSize * GridSize, CellSize * GridSize + 60);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            CreateGrid();
            InitializeHud();
            PlaceShips(2);
            StartRound();
        }


        private void CreateGrid()
        {
            for (int r = 0; r < GridSize; r++)
            {
                for (int c = 0; c < GridSize; c++)
                {
                    var btn = new Button
                    {
                        Name = $"btn_{r}_{c}",
                        Size = new Size(CellSize - 2, CellSize - 2),
                        Location = new Point(c * CellSize, r * CellSize),
                        BackColor = Color.LightBlue
                    };
                    btn.Click += Cell_Click;
                    Controls.Add(btn);
                    _cells[r, c] = btn;
                }
            }
        }

        private void InitializeHud()
        {
            lblShots = new Label
            {
                Text = "Shots: 0",
                AutoSize = true,
                Location = new Point(10, GridSize * CellSize + 15)
            };
            lblTime = new Label
            {
                Text = "Time: 0 s",
                AutoSize = true,
                Location = new Point(120, GridSize * CellSize + 15)
            };

            Controls.Add(lblShots);
            Controls.Add(lblTime);
            lblShots.BringToFront();
            lblTime.BringToFront();

            _timer.Tick += (s, e) =>
            {
                _seconds++;
                lblTime.Text = $"Time: {_seconds} s";
            };
        }

        private void PlaceShips(int count)
        {
            var rnd = new Random();
            _shipsLeft = count;

            while (count > 0)
            {
                int r = rnd.Next(GridSize);
                int c = rnd.Next(GridSize);
                if (!_ships[r, c])
                {
                    _ships[r, c] = true;
                    count--;
                }
            }
        }


        private void Cell_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null || !btn.Enabled) return;

            btn.Enabled = false;

            int[] pos = btn.Name.Split('_').Skip(1).Select(int.Parse).ToArray();
            int r = pos[0], c = pos[1];

            _shots++;
            lblShots.Text = $"Shots: {_shots}";

            if (_ships[r, c])
            {
                btn.BackColor = Color.Red;
                _shipsLeft--;
                if (_shipsLeft == 0)
                {
                    MessageBox.Show($"Перемога!\nПострілів: {_shots}", "Battleship Light");
                    ResetGame();
                }
            }
            else
            {
                btn.BackColor = Color.Gray;
            }
        }

        private void ResetGame()
        {
            Array.Clear(_ships, 0, _ships.Length);
            foreach (var b in _cells)
            {
                b.Enabled = true;
                b.BackColor = Color.LightBlue;
            }
            PlaceShips(2);
            StartRound();                
        }

        private void StartRound()
        {
            _shots = 0;
            _seconds = 0;
            lblShots.Text = "Shots: 0";
            lblTime.Text = "Time: 0 s";
            _timer.Start();
        }
    }
}
