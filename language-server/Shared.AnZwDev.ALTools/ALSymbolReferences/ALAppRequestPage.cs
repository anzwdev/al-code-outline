using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppRequestPage : ALAppPage
    {

        public ALAppRequestPage()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.RequestPage;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.name = "RequestPage";
            symbol.fullName = symbol.name;
            symbol.id = 0;
            return symbol;
        }

    }
}
