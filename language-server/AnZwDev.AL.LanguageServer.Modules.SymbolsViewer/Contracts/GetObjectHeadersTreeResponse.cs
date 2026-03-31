using AnZwDev.AL.Views.SymbolsTreeViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Contracts
{
    internal class GetObjectHeadersTreeResponse
    {

        [JsonProperty("documentUid")]
        public int DocumentUid { get; set; }

        [JsonProperty("root")]
        public SymbolsTreeNode? Root { get; set; }

    }
}
