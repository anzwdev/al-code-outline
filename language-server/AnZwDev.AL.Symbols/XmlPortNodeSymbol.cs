using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public abstract class XmlPortNodeSymbol : NamedSymbolWithProperties
    {

        public required XmlPortNodeKind Kind { get; init; }
        public required List<XmlPortNodeSymbol>? Schema { get; init; }

    }
}
