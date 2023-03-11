using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectIdRange
    {

        [JsonProperty("from")]
        public long From { get; set; }
        [JsonProperty("to")]
        public long To { get; set; }

        public ALProjectIdRange()
        {
        }

        public ALProjectIdRange(long fromId, long toId)
        {
            this.From = fromId;
            this.To = toId;
        }

    }
}
