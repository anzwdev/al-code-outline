using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ReportInformation: SymbolWithIdInformation
    {

        [JsonProperty("dataItems", NullValueHandling = NullValueHandling.Ignore)]
        public List<ReportDataItemInformation> DataItems { get; set; }

        public ReportInformation()
        {               
        }

        public ReportInformation(ALAppReport symbol): base(symbol)
        {
        }

        public ReportDataItemInformation GetReportDataItem(string name)
        {
            if (this.DataItems == null)
                return null;
            return this.DataItems
                .Where(p => (p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
                .FirstOrDefault();
        }

        public void AddReportDataItemInformation(ReportDataItemInformation reportDataItemInformation)
        {
            if (this.DataItems == null)
                this.DataItems = new List<ReportDataItemInformation>();
            this.DataItems.Add(reportDataItemInformation);
        }

        public void Sort()
        {
            if (this.DataItems != null)
            {
                foreach (ReportDataItemInformation dataItem in this.DataItems)
                {
                    dataItem.Sort();
                }
            }
        }

    }
}
