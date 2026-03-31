using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeSymbolsTreeViewProvider.Contracts
{
    public class GetSyntaxTreeSymbolsTreeViewResponse
    {

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string? Path { get; set; }

        [JsonProperty("rootNode", NullValueHandling = NullValueHandling.Ignore)]
        public SyntaxTreeSymbolsTreeViewNode? RootNode { get; set; }

    }
}
