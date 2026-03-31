using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class QuerySymbol : ObjectWithCodeSymbol
    {

        public required List<QueryDataItemSymbol>? Elements { get; init; }

        public QuerySymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.Query;
        }

    }

}
