using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppRequestPageExtension : ALAppPageExtension
    {

        public ALAppRequestPageExtension()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.RequestPageExtension;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.name = "RequestPageExtension";
            symbol.fullName = symbol.name;
            symbol.id = 0;
            return symbol;
        }

    }
}
