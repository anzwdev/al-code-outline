using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols
{
    public abstract class ALSymbolsLibrarySource
    {

        public ALSymbolsLibrarySource()
        {
        }

        public virtual ALSymbolSourceLocation GetSymbolSourceLocation(ALSymbol symbol)
        {
            return null;
        }

    }
}
