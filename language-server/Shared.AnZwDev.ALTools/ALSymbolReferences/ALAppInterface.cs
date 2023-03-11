using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppInterface : ALAppObject
    {
        public ALAppInterface()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.Interface;
        }

    }
}
