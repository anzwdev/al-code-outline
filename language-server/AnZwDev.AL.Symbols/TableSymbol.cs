using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class TableSymbol : ObjectWithCodeSymbol
    {

        public required List<TableFieldSymbol> Fields { get; init; }
        public required List<TableKeySymbol> Keys { get; init; }
        public required List<TableFieldGroupSymbol> FieldGroups { get; init; }

        private TableDataSymbol? _tableDataSymbol = null;

        public TableSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        public TableSymbol(ObjectIdentifier identifier, PropertySymbolsCollection properties)
            : base(identifier, properties)
        {
        }


        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.Table;
        }

        public TableDataSymbol GetTableDataSymbol()
        {
            if (_tableDataSymbol == null)
                _tableDataSymbol = new TableDataSymbol(this)
                { 
                    ReferenceSourceFileName = this.ReferenceSourceFileName,
                    Usings = this.Usings
                };
            return _tableDataSymbol;
        }

    }

}
