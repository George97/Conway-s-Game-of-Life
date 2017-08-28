using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public interface ICellService
    {
        void DrawCell(Cell cell);
        Cell GetNeighBourCell(Cell cell, int xDirection, int yDirection);
    }

    public class FormService : ICellService
    {
        public int CellSize { get; set; }
        public int Speed { get; set; }
        public Panel DrawPanel{ get; set; }

        private Pen _pen = new Pen(Color.White);

        public FormService(Panel drawPanel)
        {
            DrawPanel = drawPanel;
            CellSize = 20;
            Speed = 500;
        }

        public void DrawGrid()
        {
            Graphics graphics = DrawPanel.CreateGraphics();
            int numOfCellsX = DrawPanel.Width / CellSize;
            int numOfCellsY = DrawPanel.Height / CellSize;
            for (int y = 0; y <= numOfCellsY; ++y)
            {
                graphics.DrawLine(_pen, 0, y * CellSize, numOfCellsX * CellSize, y * CellSize);
            }
            for (int x = 0; x <= numOfCellsX; ++x)
            {
                graphics.DrawLine(_pen, x * CellSize, 0, x * CellSize, numOfCellsY * CellSize);
            }
        }

        public void DrawCell(Cell cell)
        {
            int i = 1;
            Brush color = cell.State == State.ALive ? Brushes.Green : Brushes.Black;
            using (Graphics g = DrawPanel.CreateGraphics())
            {
                g.FillRectangle(color, cell.X + i, cell.Y + i, CellSize - i, CellSize - i);
            }
        }

        public void ClearPanel()
        {
            DrawPanel.CreateGraphics().Clear(Color.Black);
            DrawGrid();
        }

        public Cell GetNeighBourCell(Cell cell, int xDirection, int yDirection)
        {
            int x = cell.X + xDirection * CellSize;
            int y = cell.Y + yDirection * CellSize;
            bool hasNeighbours = x > 0 && x < DrawPanel.Width && y > 0 && y < DrawPanel.Height;
            return hasNeighbours ? new Cell() { X = x, Y = y } : null;
        }
    }
}
