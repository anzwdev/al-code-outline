using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class EnumValueSymbol : NamedSymbolWithProperties
    {

        public required int Ordinal { get; init; }

    }
}
