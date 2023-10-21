using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols
{
    public class PositionComparer : IComparer<Position>
    {
        public int Compare(Position x, Position y)
        {
            if ((x == null) && (y == null))
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            
            var val = x.line - y.line;
            if (val == 0)
                val = x.character - y.character;

            return val;
        }
    }
}
