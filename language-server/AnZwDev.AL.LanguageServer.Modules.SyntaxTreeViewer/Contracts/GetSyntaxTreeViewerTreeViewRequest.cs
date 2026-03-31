using AnZwDev.AL.Views.SyntaxTreeViewerTreeViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeViewer.Contracts
{
    public class GetSyntaxTreeViewerTreeViewRequest
    {

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string? Path { get; set; }

        [JsonProperty("viewMode")]
        public SyntaxTreeViewerViewMode ViewMode { get; set; }

    }

}
