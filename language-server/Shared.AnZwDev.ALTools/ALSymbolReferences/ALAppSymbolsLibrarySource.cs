using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppSymbolsLibrarySource : ALAppBaseSymbolsLibrarySource
    {

        public ALAppSymbolReference SymbolReference { get; private set; }

        public ALAppSymbolsLibrarySource(ALAppSymbolReference symbolReference)
        {
            this.SymbolReference = symbolReference;
        }

        public override ALSymbolSourceLocation GetSymbolSourceLocation(ALSymbol symbol)
        {
            ALSymbolSourceLocation location = new ALSymbolSourceLocation(symbol);
            ALAppObject alAppObject = this.SymbolReference.FindObjectByName(symbol.kind, symbol.name, false);
            if (alAppObject != null)
                this.SetSource(location, this.SymbolReference, alAppObject);
            return location;
        }

    }
}
