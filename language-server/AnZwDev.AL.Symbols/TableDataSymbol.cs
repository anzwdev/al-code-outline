using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class TableDataSymbol : ObjectSymbol
    {

        public TableSymbol Table { get; }

        public TableDataSymbol(TableSymbol tableSymbol) : 
            base(tableSymbol.Identifier.CreateTableDataIdentifier(), tableSymbol.Properties)
        {
            Table = tableSymbol;
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.TableData;
        }

    }
}
