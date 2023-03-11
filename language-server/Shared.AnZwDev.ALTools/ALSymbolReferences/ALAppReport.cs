using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppReport : ALAppObject
    {

        public ALAppRequestPage RequestPage { get; set; }
        public ALAppElementsCollection<ALAppReportDataItem> DataItems { get; set; }

        public ALAppReport()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ReportObject;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.DataItems?.AddToALSymbol(symbol, ALSymbolKind.ReportDataSetSection, "dataset");
            if (this.RequestPage != null)
                symbol.AddChildSymbol(this.RequestPage.ToALSymbol());
            base.AddChildALSymbols(symbol);
        }

        public ALAppReportDataItem FindDataItem(string name)
        {
            if ((this.DataItems != null) && (!String.IsNullOrWhiteSpace(name)))
                return this.FindDataItem(this.DataItems, name);
            return null;
        }

        protected ALAppReportDataItem FindDataItem(ALAppElementsCollection<ALAppReportDataItem> dataItemsCollection, string name)
        {
            foreach (ALAppReportDataItem dataItem in dataItemsCollection)
            {
                if (name.Equals(dataItem.Name, StringComparison.CurrentCultureIgnoreCase))
                    return dataItem;
                if (dataItem.DataItems != null)
                {
                    ALAppReportDataItem foundDataItem = this.FindDataItem(dataItem.DataItems, name);
                    if (foundDataItem != null)
                        return foundDataItem;
                }
            }
            return null;
        }

    }
}
