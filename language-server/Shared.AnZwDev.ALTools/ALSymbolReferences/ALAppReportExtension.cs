using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppReportExtension : ALAppObject, IALAppObjectExtension
    {

        public string Target { get; set; }
        public ALAppRequestPageExtension RequestPage { get; set; }
        public ALAppSymbolsCollection<ALAppReportDataItem> DataItems { get; set; }
        public ALAppSymbolsCollection<ALAppReportColumn> Columns { get; set; }

        public ALAppReportExtension()
        {
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


        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ReportExtensionObject;
        }

        public override ALObjectType GetALObjectType()
        {
            return ALObjectType.ReportExtension;
        }


        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.extends = this.Target;
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            //this.Columns?.AddToALSymbol(symbol, ALSymbolKind.ReportExtensionAddColumnChange, "columns");            
            if ((this.Columns != null) && (this.Columns.Count > 0))
                this.AddColumns(symbol);
            this.DataItems?.AddToALSymbol(symbol, ALSymbolKind.ReportExtensionAddDataItemChange, "dataItems");
            if (this.RequestPage != null)
                symbol.AddChildSymbol(this.RequestPage.ToALSymbol());
            base.AddChildALSymbols(symbol);
        }

        protected void AddColumns(ALSymbol symbol)
        {
            Dictionary<string, ALSymbol> dataItems = new Dictionary<string, ALSymbol>();
            ALSymbol columnsSymbol = new ALSymbol(ALSymbolKind.ReportExtensionAddColumnChange, "columns");

            foreach (ALAppReportColumn column in this.Columns)
            {
                string parentName = column.OwningDataItemName;
                if (String.IsNullOrWhiteSpace(parentName))
                    parentName = "Undefined";
                string key = parentName.ToLower();

                ALSymbol dataItemSymbol = null;
                if (dataItems.ContainsKey(key))
                    dataItemSymbol = dataItems[key];
                else
                {
                    dataItemSymbol = new ALSymbol(ALSymbolKind.ReportDataItem, parentName);
                    dataItems.Add(key, dataItemSymbol);
                    columnsSymbol.AddChildSymbol(dataItemSymbol);
                }

                dataItemSymbol.AddChildSymbol(column.ToALSymbol());
            }

            symbol.AddChildSymbol(columnsSymbol);
        }

        public string GetTargetObjectName()
        {
            return this.Target;
        }

        public ALObjectReference GetTargetObjectReference()
        {
            return new ALObjectReference(Usings, this.Target);
        }

    }
}
