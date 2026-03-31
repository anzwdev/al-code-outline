using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public enum PageActionKind
    {

        Area = 0,
        Group = 1,
        Action = 2,
        Separator = 3,
        ActionRef = 4,
        CustomAction = 5,
        SystemAction = 6,
        FileUploadAction = 7,

        Undefined = 1000
    }
}
