using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class TableKeySymbol : NamedSymbol
    {

        public required PropertySymbolsCollection Properties { get; init; }
        public required List<string> FieldNames { get; set; }

    }
}
