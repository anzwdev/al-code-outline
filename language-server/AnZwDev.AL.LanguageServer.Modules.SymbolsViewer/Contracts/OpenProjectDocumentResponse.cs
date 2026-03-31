using AnZwDev.AL.Views.SymbolsTreeViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Contracts
{
    internal class OpenProjectDocumentResponse
    {

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string? Path { get; set; }

        [JsonProperty("includeDependencies", NullValueHandling = NullValueHandling.Ignore)]
        public bool IncludeDependencies { get; set; }

        [JsonProperty("documentUid")]
        public int DocumentUid { get; set; }

        [JsonProperty("root")]
        public SymbolsTreeNode? Root { get; set; }

    }
}
