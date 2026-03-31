using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class PageExtensionSymbol : ObjectExtensionWithCodeSymbol
    {

        public required List<PageControlChangeSymbol>? ControlChanges { get; init; }
        public required List<PageActionChangeSymbol>? ActionChanges { get; init; }
        public required List<PageViewChangeSymbol>? ViewChanges { get; init; }

        public PageExtensionSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.PageExtension;
        }

        protected override ObjectKind GetExtendedObjectType()
        {
            return ObjectKind.Page;
        }

    }

}
