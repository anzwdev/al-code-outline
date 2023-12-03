using AnZwDev.ALTools.ALSymbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class CollectWorkspaceCommandCodeActionsRequest
    {

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("projectPath")]
        public string ProjectPath { get; set; }
        
        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        [JsonProperty("range")]
        public Range Range { get; set; }

    }
}
