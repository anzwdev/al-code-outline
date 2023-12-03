using AnZwDev.ALTools.ALSymbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class WorkspaceCommandCodeAction
    {

        [JsonProperty("commandName")]
        public string CommandName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("range")]
        public Range Range { get; set; }

        [JsonProperty("parameters", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

        public WorkspaceCommandCodeAction()
        {
        }

        public WorkspaceCommandCodeAction(string commandName, Range range, string description)
        {
            CommandName = commandName;
            Range = range;
            Description = description;
        }

    }
}
