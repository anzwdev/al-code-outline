using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class PageInformation : TableBasedSymbolWithIdInformation
    {

        [JsonProperty("applicationArea")]
        public string ApplicationArea { get; set; }

        [JsonProperty("namespace", NullValueHandling = NullValueHandling.Ignore)]
        public string Namespace { get; set; }

        public PageInformation()
        {
        }

        public PageInformation(ALAppPage page) : base(page, null)
        {
            Namespace = page.NamespaceName;
            if (page.Properties != null)
            {
                this.Caption = page.Properties.GetStringValue("Caption");
                this.Source = page.Properties.GetRawValue("SourceTable");
                this.ApplicationArea = page.Properties.GetRawValue("ApplicationArea");
            }
        }

    }
}
