using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.Workspace.Serialization
{
    public partial class ALProjectMetadataSerializer
    {

        private class ProjectIdRangeMetadata
        {

            [JsonProperty("from")]
            public long From { get; set; }

            [JsonProperty("to")]
            public long To { get; set; }

        }

    }
}
