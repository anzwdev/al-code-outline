using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeViewer.Contracts
{
    public class GetSyntaxTreeViewerTreeNodePropertiesRequest
    {

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string? Path { get; set; }

        [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
        public string? Uid { get; set; }

    }
}
