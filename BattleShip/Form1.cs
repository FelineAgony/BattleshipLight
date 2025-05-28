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
        private const int CellSize = 60;
        private readonly Button[,] _cells;
        private readonly Timer _timer;
        private readonly GameEngine _engine;

        private Label lblShots, lblTime, lblShipsLeft;

        public Form1()
        {
            InitializeComponent();

            _engine = new GameEngine(gridSize: 5);
            _cells = new Button[_engine.GridSize, _engine.GridSize];
            _timer = new Timer { Interval = 1000 };
            _timer.Tick += (s, e) => OnTimerTick();

            Text = "Battleship Light";
            ClientSize = new Size(_engine.GridSize * CellSize,
                                  _engine.GridSize * CellSize + 60);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            CreateGrid();
            InitializeHud();
            NewGame();
        }

        private void CreateGrid()
        {
            for (int r = 0; r < _engine.GridSize; r++)
                for (int c = 0; c < _engine.GridSize; c++)
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

        private void InitializeHud()
        {
            lblShots = new Label
            {
                AutoSize = true,
                Location = new Point(10, _engine.GridSize * CellSize + 15)
            };
            lblTime = new Label
            {
                AutoSize = true,
                Location = new Point(120, _engine.GridSize * CellSize + 15)
            };
            lblShipsLeft = new Label
            {
                AutoSize = true,
                Location = new Point(240, _engine.GridSize * CellSize + 15)
            };

            Controls.AddRange(new Control[] { lblShots, lblTime, lblShipsLeft });
        }

        private void OnTimerTick()
        {
            _engine.TickSecond();
            lblTime.Text = $"Time: {_engine.ElapsedSeconds} s";
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (!btn.Enabled) return;
            btn.Enabled = false;

            var coords = btn.Name.Split('_').Skip(1).Select(int.Parse).ToArray();
            bool hit = _engine.Shoot(coords[0], coords[1]);

            btn.BackColor = hit ? Color.Red : Color.Gray;
            UpdateHud();

            if (_engine.ShipsLeft == 0)
            {
                _timer.Stop();
                MessageBox.Show(
                    $"Victory!\nShots: {_engine.ShotsCount}\nTime: {_engine.ElapsedSeconds} s",
                    "Battleship Light");
                NewGame();
            }
        }

        private void NewGame()
        {
            foreach (var b in _cells)
            {
                b.Enabled = true;
                b.BackColor = Color.LightBlue;
            }

            _engine.StartNewGame(shipCount: 2);
            UpdateHud();
            _timer.Start();
        }

        private void UpdateHud()
        {
            lblShots.Text = $"Shots: {_engine.ShotsCount}";
            lblShipsLeft.Text = $"Ships left: {_engine.ShipsLeft}";
            lblTime.Text = $"Time: {_engine.ElapsedSeconds} s";
        }
    }
}