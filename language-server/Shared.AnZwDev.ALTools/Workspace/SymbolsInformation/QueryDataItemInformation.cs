using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class QueryDataItemInformation : TableBasedSymbolInformation
    {

        public QueryDataItemInformation()
        {
        }

        public QueryDataItemInformation(ALAppQueryDataItem dataItem) : base(dataItem, dataItem.RelatedTable)
        {
        }

    }
}
