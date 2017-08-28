using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public enum State { Dead, ALive };

    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Neighbours { get; set; }
        public State State { get; set; }

        public Cell() { }

        public Cell(Cell cell,State state)
        {
            X = cell.X;
            Y = cell.Y;
            Neighbours = cell.Neighbours;
            State = state;
        }

        public bool IsAllive()
        {
            return State == State.ALive;
        }


    }
}
