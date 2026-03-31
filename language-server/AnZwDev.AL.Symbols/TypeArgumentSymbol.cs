using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class TypeArgumentSymbol : NamedSymbol
    {

        public required SubtypeSymbol? Subtype { get; init; }
        public required List<TypeArgumentSymbol>? TypeArguments { get; init; }

    }
}
