using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class TableFieldGroupExtensionSymbol : Symbol
    {

        public required string? Anchor { get; init; }
        public required List<string> FieldNames { get; init; }

    }
}
