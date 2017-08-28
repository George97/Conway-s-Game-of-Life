using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public interface IRules
    {
        Cell Apply(Cell cell);
    }

    public class ConwayRules: IRules
    {
        public Cell Apply(Cell cell)
        {
            if(cell.Neighbours < 2 || cell.Neighbours > 3)
            {
                return new Cell(cell, State.Dead);
            }

            switch(cell.State)
            {
                case State.Dead:
                    return cell.Neighbours == 3 ? 
                        new Cell(cell,State.ALive) : new Cell(cell, State.Dead);

                case State.ALive:
                    return cell.Neighbours == 2 || cell.Neighbours == 3 ?
                        new Cell(cell, State.ALive) : new Cell(cell, State.Dead);

                default:
                    return new Cell(cell, State.Dead);
            }
        }
    }
}
