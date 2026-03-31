using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class NamedSymbolWithProperties : NamedSymbol
    {

        public required PropertySymbolsCollection? Properties { get; init; }

    }
}
