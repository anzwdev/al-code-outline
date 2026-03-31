using AnZwDev.AL.Views.SyntaxTreeTreeViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeTreeViewProvider.Contracts
{
    public class GetSyntaxTreeTreeViewResponse
    {

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string? Path { get; set; }

        [JsonProperty("rootNode", NullValueHandling = NullValueHandling.Ignore)]
        public SyntaxTreeTreeViewNode? RootNode { get; set; }

    }
}
