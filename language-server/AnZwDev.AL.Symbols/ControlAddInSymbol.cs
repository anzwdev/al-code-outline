using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class ControlAddInSymbol : ObjectSymbol
    {

        public required string? PublicKeyToken { get; init; }
        public required string? MetadataName { get; init; }
        public required List<MethodSymbol>? Methods { get; init; }
        public required List<EventSymbol>? Events { get; init; }

        public ControlAddInSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties) : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.ControlAddIn;
        }

    }

}
