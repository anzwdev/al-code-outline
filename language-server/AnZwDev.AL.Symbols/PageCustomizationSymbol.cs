using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class PageCustomizationSymbol : ObjectExtensionSymbol
    {

        public required List<PageControlChangeSymbol>? ControlChanges { get; init; }

        public PageCustomizationSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.PageCustomization;
        }

        protected override ObjectKind GetExtendedObjectType()
        {
            return ObjectKind.Page;
        }

    }

}
