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

        private readonly Button[,] _cells = new Button[GridSize, GridSize];
        private readonly bool[,] _ships = new bool[GridSize, GridSize];
        private int _shipsLeft;

        private int _shots;
        private Label lblShots;

        public Form1()
        {
            Text = "Battleship Light";
            ClientSize = new Size(300, 320);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            CreateGrid();
            PlaceShips(2);
            InitializeShotCounter();
        }


        private void CreateGrid()
        {
            int cell = ClientSize.Width / GridSize;

            for (int r = 0; r < GridSize; r++)
            {
                for (int c = 0; c < GridSize; c++)
                {
                    var btn = new Button
                    {
                        Name = $"btn_{r}_{c}",
                        Size = new Size(cell - 2, cell - 2),
                        Location = new Point(c * cell, r * cell),
                        BackColor = Color.LightBlue,
                        Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold)
                    };
                    btn.Click += Cell_Click;
                    Controls.Add(btn);
                    _cells[r, c] = btn;
                }
            }
        }

        private void InitializeShotCounter()
        {
            lblShots = new Label
            {
                Text = "Shots: 0",
                AutoSize = true,
                Location = new Point(10, ClientSize.Height - 40)
            };
            Controls.Add(lblShots);
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
            if (btn == null) return;

            int[] pos = btn.Name.Split('_').Skip(1).Select(int.Parse).ToArray();
            int r = pos[0], c = pos[1];

            if (!btn.Enabled) return;
            btn.Enabled = false;

            _shots++;
            lblShots.Text = $"Shots: {_shots}";

            if (_ships[r, c])
            {
                btn.BackColor = Color.Red;
                btn.Text = "X";
                _shipsLeft--;

                if (_shipsLeft == 0)
                {
                    MessageBox.Show("WIN",
                                    "Battleship Light");
                    ResetGame();
                }
            }
            else
            {
                btn.BackColor = Color.Gray;
                btn.Text = "•";
            }
        }


        private void ResetGame()
        {
            Array.Clear(_ships, 0, _ships.Length);
            foreach (var b in _cells)
            {
                b.Enabled = true;
                b.BackColor = Color.LightBlue;
                b.Text = string.Empty;
            }

            PlaceShips(2);

            _shots = 0;                 
            lblShots.Text = "Shots: 0";
        }
    }
}
