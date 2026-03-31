using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class PageControlSymbol : NamedSymbolWithIdAndProperties
    {

        public required PageControlKind Kind { get; init; }
        public required List<PageControlSymbol>? Controls { get; init; }
        public required List<PageActionSymbol>? Actions { get; init; }
        public required TypeDefinitionSymbol? TypeDefinition { get; init; }
        public required ObjectReference? RelatedPagePartId { get; init; }
        public required ObjectReference? RelatedControlAddIn { get; init; }
        public required string? RelatedControlAddInPublicKey { get; init; }

    }
}
