using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppQuery : ALAppObject
    {

        public ALAppElementsCollection<ALAppQueryDataItem> Elements { get; set; }

        public ALAppQuery()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.QueryObject;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Elements?.AddToALSymbol(symbol, ALSymbolKind.QueryElements, "elements");
            base.AddChildALSymbols(symbol);
        }

        public ALAppQueryDataItem FindDataItem(string name)
        {
            if ((this.Elements != null) && (!String.IsNullOrWhiteSpace(name)))
                return this.FindDataItem(this.Elements, name);
            return null;
        }

        protected ALAppQueryDataItem FindDataItem(ALAppElementsCollection<ALAppQueryDataItem> dataItemsCollection, string name)
        {
            foreach (ALAppQueryDataItem dataItem in dataItemsCollection)
            {
                if (name.Equals(dataItem.Name, StringComparison.CurrentCultureIgnoreCase))
                    return dataItem;
                if (dataItem.DataItems != null)
                {
                    ALAppQueryDataItem foundDataItem = this.FindDataItem(dataItem.DataItems, name);
                    if (foundDataItem != null)
                        return foundDataItem;
                }
            }
            return null;
        }

    }
}
