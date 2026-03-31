using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class EnumTypeSymbol : ObjectSymbol
    {

        public required List<EnumValueSymbol>? Values { get; init; }
        public required List<ObjectReference>? ImplementedInterfaces { get; init; }

        public EnumTypeSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties) : 
            base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.EnumType;
        }

    }

}
