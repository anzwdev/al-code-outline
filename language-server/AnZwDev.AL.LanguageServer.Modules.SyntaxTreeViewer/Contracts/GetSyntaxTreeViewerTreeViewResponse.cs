using AnZwDev.AL.Views.SyntaxTreeViewerTreeViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeViewer.Contracts
{
    public class GetSyntaxTreeViewerTreeViewResponse
    {

        [JsonProperty("rootNode", NullValueHandling = NullValueHandling.Ignore)]
        public SyntaxTreeViewerTreeNode? RootNode { get; set; }

    }
}
