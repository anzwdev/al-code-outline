using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class TableExtensionSymbol : ObjectExtensionWithCodeSymbol
    {

        public required List<TableFieldSymbol>? Fields { get; init; }
        public required List<TableKeySymbol>? Keys { get; init; }
        public required List<TableFieldGroupExtensionSymbol>? FieldGroups { get; init; }

        public TableExtensionSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetExtendedObjectType()
        {
            return ObjectKind.Table;
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.TableExtension;
        }


    }

}
