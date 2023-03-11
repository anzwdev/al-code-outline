using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{

    [Flags]
    public enum ALAppPermissionValue
    {

        R = 1,
        I = 2,
        M = 4,
        D = 8,
        X = 16,
        IndirectR = 32,
        IndirectI = 64,
        IndirectM = 128,
        IndirectD = 256,
        IndirectX = 512,

        Empty = 0,
        AllDirect = R | I | M | D | X,
        AllIndirect = IndirectR | IndirectI | IndirectM | IndirectD | X,
        AllDirectTableData = R | I | M | D,
        AllIndirectTableData = IndirectR | IndirectI | IndirectM | IndirectD

    }
}
