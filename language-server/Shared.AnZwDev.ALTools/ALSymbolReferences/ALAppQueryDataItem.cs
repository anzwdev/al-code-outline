using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppQueryDataItem : ALAppElementWithName
    {

        public string RelatedTable { get; set; }
        public ALAppElementsCollection<ALAppQueryDataItem> DataItems { get; set; }
        public ALAppElementsCollection<ALAppQueryColumn> Columns { get; set; }
        public ALAppElementsCollection<ALAppQueryFilter> Filters { get; set; }

        public ALAppQueryDataItem()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.QueryDataItem;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.fullName = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(this.Name);
            if (!String.IsNullOrWhiteSpace(this.RelatedTable))
                symbol.fullName = symbol.fullName + ": Table " + this.RelatedTable;
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.DataItems?.AddToALSymbol(symbol);
            this.Columns?.AddToALSymbol(symbol);
            this.Filters?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }


    }
}
