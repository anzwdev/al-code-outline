using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class PermissionSetSymbol : ObjectSymbol
    {

        public required List<PermissionSymbol>? Permissions { get; init; }
        public required List<ObjectReference>? IncludedPermissionSets { get; init; }
        public required List<ObjectReference>? ExcludedPermissionSets { get; init; }

        public PermissionSetSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.PermissionSet;
        }

    }

}
