using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class LifeProcess
    {
        public List<Cell> OldGeneration { get; set; }
        public List<Cell> NewGeneration { get; set; }
        public IRules Rules { get; set; }
        private ICellService _cellService { get; set; }
        private List<Cell> _neighbours;

        public LifeProcess(ICellService cellService, IRules rules)
        {
            OldGeneration = new List<Cell>();
            NewGeneration = new List<Cell>();
            _cellService = cellService;
            _neighbours = new List<Cell>();
            Rules = rules;
        }

        public void Cycle()
        {
            if (OldGeneration.Count > 0)
            {
                for (int i = 0; i < OldGeneration.Count; i++)
                {
                    SetNeighbours(OldGeneration[i]);
                }
                OldGeneration.AddRange(_neighbours);
                _neighbours.Clear();

                foreach (var cell in OldGeneration)
                {
                    Cell newCell = Rules.Apply(cell);
                    _cellService.DrawCell(newCell);
                    if(newCell.State == State.ALive)
                    {
                        newCell.Neighbours = 0;
                        NewGeneration.Add(newCell);
                    }
                }
                OldGeneration = new List<Cell>(NewGeneration);
                NewGeneration.Clear();
            }
        }

        public void SetInitialState()
        {
            OldGeneration.Clear();
            NewGeneration.Clear();
            _neighbours.Clear();
        }

        private void SetNeighbours(Cell cell)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue; // neighbour = cell
                    Cell neighbour = _cellService.GetNeighBourCell(cell, i, j);
                    if(neighbour != null)
                    {
                        var existingCell = OldGeneration.FirstOrDefault(c => c.X == neighbour.X && c.Y == neighbour.Y);
                        existingCell = existingCell == null ? _neighbours.FirstOrDefault(c => c.X == neighbour.X && c.Y == neighbour.Y) : existingCell;
                        // is dead
                        if (existingCell == null)
                        {
                            neighbour.State = State.Dead;
                            neighbour.Neighbours = 1;
                            _neighbours.Add(neighbour);
                        }
                        // only forn next alive element
                        else if (OldGeneration.IndexOf(existingCell) > OldGeneration.IndexOf(cell))
                        {
                            ++existingCell.Neighbours;
                            ++cell.Neighbours;
                        }
                        // existing dead element
                        if(existingCell!= null && _neighbours.Contains(existingCell))
                        {
                            ++existingCell.Neighbours;
                        }
                    }
                }
            }
        }
    }
}
