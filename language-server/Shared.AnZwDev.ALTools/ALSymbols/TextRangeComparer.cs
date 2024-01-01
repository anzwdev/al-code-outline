using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols
{
    public class TextRangeComparer : IComparer<TextRange>
    {

        private readonly PositionComparer _positionComparer = new PositionComparer();

        public int Compare(TextRange x, TextRange y)
        {
            if ((x == null) && (y == null))
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            var val = _positionComparer.Compare(x.start, y.start);
            if (val == 0)
                val = _positionComparer.Compare(y.end, x.end);  //start the same, check end but invert result as lower end means that symbol is inside another

            return val;
        }
    }
}
