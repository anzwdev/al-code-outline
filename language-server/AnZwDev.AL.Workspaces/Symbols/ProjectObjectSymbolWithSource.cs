using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.Symbols
{

    public struct ProjectObjectSymbolWithSource<T> where T : ObjectSymbol
    {

        public T? Symbol { get; set; }
        public ApplicationSymbol? Source { get; set; }

    }
}
