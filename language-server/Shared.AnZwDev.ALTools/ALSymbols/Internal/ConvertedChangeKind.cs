using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.Internal
{
    internal enum ConvertedChangeKind
    {
        Add = 0,
        AddFirst = 1,
        AddLast = 2,
        AddBefore = 3,
        AddAfter = 4,
        MoveFirst = 5,
        MoveLast = 6,
        MoveBefore = 7,
        MoveAfter = 8,
        Modify = 9
    }
}
