using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class PageViewChangeSymbol : Symbol
    {

        public required string? Anchor { get; init; }
        public required PageViewChangeKind ChangeKind { get; init; }
        public required List<PageViewSymbol>? Views { get; init; }

    }
}
