using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces.Symbols
{
    public struct ApplicationSymbolDescriptor
    {

        public ApplicationSymbol Symbol { get; set; }
        public AccessLevelFilter AccessLevelFilter { get; set; }

    }
}
