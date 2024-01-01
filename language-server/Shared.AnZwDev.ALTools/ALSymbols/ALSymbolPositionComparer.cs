using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSymbolPositionComparer : IComparer<ALSymbol>
    {

        private readonly TextRangeComparer _rangeComparer = new TextRangeComparer();

        public int Compare(ALSymbol x, ALSymbol y)
        {
            if ((x == null) && (y == null))
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            return _rangeComparer.Compare(x.range, y.range);
        }
    }
}
