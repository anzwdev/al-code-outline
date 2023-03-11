using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppCodeunit : ALAppObject
    {

        public ALAppCodeunit()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.CodeunitObject;
        }

    }
}
