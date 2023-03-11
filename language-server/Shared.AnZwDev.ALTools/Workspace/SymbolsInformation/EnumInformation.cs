using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class EnumInformation : SymbolInformation
    {

        public EnumInformation()
        {
        }

        public EnumInformation(ALAppEnum symbol) : base(symbol)
        {
            //this.Implements = symbol.Implements;
        }

    }
}
