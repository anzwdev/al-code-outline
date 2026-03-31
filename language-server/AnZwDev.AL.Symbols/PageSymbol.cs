using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class PageSymbol : ObjectWithCodeSymbol
    {

        public required bool HasActionsV2 { get; init; }
        public required List<PageControlSymbol>? Controls { get; init; }
        public required List<PageActionSymbol>? Actions { get; init; }
        public required List<PageViewSymbol>? Views { get; init; }
        public required ObjectReference SourceTable { get; init; }

        public PageSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.Page;
        }

    }

}
