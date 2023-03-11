using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppReportDataItem : ALAppElementWithName
    {
        
        public string OwningDataItemName { get; set; }
        public string RelatedTable { get; set; }
        public ALAppElementsCollection<ALAppReportColumn> Columns { get; set; }
        public ALAppElementsCollection<ALAppReportDataItem> DataItems { get; set; }

        public ALAppReportDataItem()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ReportDataItem;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            if (!String.IsNullOrWhiteSpace(this.RelatedTable))
                symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + ALSyntaxHelper.EncodeName(this.RelatedTable);
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Columns?.AddToALSymbol(symbol);
            this.DataItems?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
