using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class ReportDataItemSymbol : NamedSymbolWithIdAndProperties
    {

        public required string? OwningDataItemName { get; init; }
        public required ObjectReference? RelatedTable { get; init; }
        public required int Indentation { get; init; }
        public required int FilterControlId { get; init; }
        public required List<ReportColumnSymbol>? Columns { get; init; }
        public required List<ReportDataItemSymbol>? DataItems { get; init; }


    }
}
