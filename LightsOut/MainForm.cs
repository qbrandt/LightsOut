using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private const int GridOffset = 25;    // Distance from upper-left side of window         
        private int GridLength;   // Size in pixels of grid              
        private int CellLength { get { return GridLength / lightsOutGame.GridSize; } }

        private LightsOutGame lightsOutGame;

        public MainForm()
        {
            InitializeComponent();

            lightsOutGame = new LightsOutGame(); //Make sure this is initialized before grid caluculations happen or you'll get a null reference error

            GridLength = Math.Min(this.Width, this.Height - 100) - 2 * GridOffset;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int r = 0; r < lightsOutGame.GridSize; r++)
            {
                for (int c = 0; c < lightsOutGame.GridSize; c++)
                {
                    Brush brush;
                    Pen pen;

                    if (lightsOutGame.GetGridValue(r, c))
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;
                    }

                    int x = c * CellLength + GridOffset;
                    int y = r * CellLength + GridOffset;

                    g.DrawRectangle(pen, x, y, CellLength, CellLength);
                    g.FillRectangle(brush, x + 1, y + 1, CellLength - 1, CellLength - 1);
                }
            }

        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < GridOffset || e.X > CellLength * lightsOutGame.GridSize + GridOffset ||
                e.Y < GridOffset || e.Y > CellLength * lightsOutGame.GridSize + GridOffset)
            {
                return;
            }

            int r = (e.Y - GridOffset) / CellLength;
            int c = (e.X - GridOffset) / CellLength;

            lightsOutGame.Move(r, c);

            this.Invalidate();

            if (lightsOutGame.IsGameOver())
            {
                MessageBox.Show(this, "Congratulations!  You've won!", "Lights Out!", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            lightsOutGame.NewGame();

            this.Invalidate();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGameButton_Click(sender, e);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitButton_Click(sender, e);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            GridLength = Math.Min(this.Width, this.Height - 100) - 2 * GridOffset;
            this.Invalidate();
        }
    }
}
