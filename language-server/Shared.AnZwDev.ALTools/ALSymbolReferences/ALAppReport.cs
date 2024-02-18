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
        public ALAppSymbolsCollection<ALAppReportDataItem> DataItems { get; set; }

        public ALAppReport()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ReportObject;
        }

        public override ALObjectType GetALObjectType()
        {
            return ALObjectType.Report;
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

        protected ALAppReportDataItem FindDataItem(ALAppSymbolsCollection<ALAppReportDataItem> dataItemsCollection, string name)
        {
            foreach (ALAppReportDataItem dataItem in dataItemsCollection)
            {
                if (name.Equals(dataItem.Name, StringComparison.OrdinalIgnoreCase))
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
