using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class PageActionChangeSymbol : Symbol
    {

        public required string? Anchor { get; init; }
        public required PageActionChangeKind ChangeKind { get; init; }
        public required PropertySymbolsCollection Properties { get; init; }
        public required List<PageActionSymbol>? Actions { get; init; }

    }
}
