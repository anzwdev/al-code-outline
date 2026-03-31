using AnZwDev.AL.Views.SyntaxTreeViewerTreeViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeViewer.Contracts
{
    public class GetSyntaxTreeViewerTreeNodePropertiesResponse
    {

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public List<SyntaxTreeViewerTreeNodeProperty>? Properties { get; set; }

    }
}
