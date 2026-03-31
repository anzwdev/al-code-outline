using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class EnumExtensionTypeSymbol : ObjectExtensionSymbol
    {

        public required List<EnumValueSymbol>? Values { get; init; }

        public EnumExtensionTypeSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetExtendedObjectType()
        {
            return ObjectKind.EnumType;
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.EnumExtensionType;
        }


    }

}
