using AnZwDev.ALTools.WorkspaceCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class CollectWorkspaceCommandCodeActionsResponse
    {

        [JsonProperty("codeActions", NullValueHandling = NullValueHandling.Ignore)]
        public List<WorkspaceCommandCodeAction> CodeActions { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("errorMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }

    }
}
