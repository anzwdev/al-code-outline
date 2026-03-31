using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class PermissionSetExtensionSymbol : ObjectExtensionSymbol
    {

        public required List<PermissionSymbol>? Permissions { get; init; }
        public required List<ObjectReference>? IncludedPermissionSets { get; init; }
        public required List<ObjectReference>? ExcludedPermissionSets { get; init; }

        public PermissionSetExtensionSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetExtendedObjectType()
        {
            return ObjectKind.PermissionSet;
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.PermissionSetExtension;
        }


    }

}
