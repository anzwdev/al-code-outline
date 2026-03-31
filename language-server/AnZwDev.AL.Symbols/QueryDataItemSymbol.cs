using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class QueryDataItemSymbol : NamedSymbolWithIdAndProperties
    {

        public required ObjectReference? RelatedTable { get; init; }
        public required List<QueryDataItemSymbol>? DataItems { get; init; }
        public required List<QueryColumnSymbol>? Columns { get; init; }
        public required List<QueryColumnSymbol>? Filters { get; init; }


    }
}
