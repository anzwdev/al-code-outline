using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public class TableSymbolsCollection : ObjectSymbolsCollection<TableSymbol>
    {

        private ObjectSymbolsCollection<TableDataSymbol> _tableDataSymbolsCollection;
        public ReadOnlyObjectSymbolsCollection<TableDataSymbol> TableData { get; }

        public TableSymbolsCollection()
        {
            _tableDataSymbolsCollection = new ObjectSymbolsCollection<TableDataSymbol>();
            TableData = new ReadOnlyObjectSymbolsCollection<TableDataSymbol>(_tableDataSymbolsCollection);
        }

        protected override void OnClear()
        {
            base.OnClear();
            _tableDataSymbolsCollection.Clear();
        }

        protected override void OnItemAdded(TableSymbol item)
        {
            base.OnItemAdded(item);
            _tableDataSymbolsCollection.Add(item.GetTableDataSymbol());
        }

        protected override void OnItemRemoved(TableSymbol item)
        {
            base.OnItemRemoved(item);
            _tableDataSymbolsCollection.Remove(item.GetTableDataSymbol());
        }

    }
}
