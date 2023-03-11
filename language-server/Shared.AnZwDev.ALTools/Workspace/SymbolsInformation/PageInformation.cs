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

        public PageInformation()
        {
        }

        public PageInformation(ALAppPage page) : base(page, null)
        {
            if (page.Properties != null)
            {
                this.Caption = page.Properties.GetValue("Caption");
                this.Source = page.Properties.GetValue("SourceTable");
                this.ApplicationArea = page.Properties.GetValue("ApplicationArea");
            }
        }

    }
}
