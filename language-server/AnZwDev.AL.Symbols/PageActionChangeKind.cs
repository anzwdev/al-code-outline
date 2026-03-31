using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public enum PageActionChangeKind
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
        Modify = 9,

        Undefined = 1000

    }
}
