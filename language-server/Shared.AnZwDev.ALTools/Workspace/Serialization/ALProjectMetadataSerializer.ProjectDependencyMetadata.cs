using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.Workspace.Serialization
{
    public partial class ALProjectMetadataSerializer
    {

        private class ProjectDependencyMetadata
        {

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("appId")]
            public string AppId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("publisher")]
            public string Publisher { get; set; }

            [JsonProperty("version")]
            public string Version { get; set; }

            public ProjectDependencyMetadata()
            {
            }

            public string GetId()
            {
                if (!String.IsNullOrWhiteSpace(this.Id))
                    return this.Id;
                return this.AppId;
            }


        }

    }
}
