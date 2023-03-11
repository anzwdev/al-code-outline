using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ReportDataItemInformation : TableBasedSymbolInformation
    {

        [JsonProperty("indent")]
        public int Indent { get; set; }

        public ReportDataItemInformation()
        {
            this.Indent = 0;
        }

        public ReportDataItemInformation(ALAppReportDataItem dataItemSymbol) : base(dataItemSymbol, dataItemSymbol.RelatedTable)
        {
            this.Indent = 0;
        }

    }
}
