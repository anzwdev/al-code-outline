using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSymbolSourceLocation
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string schema { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sourcePath { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Range range { get; set; }

        public ALSymbolSourceLocation()
        {
            this.schema = null;
            this.sourcePath = null;
        }

        public ALSymbolSourceLocation(ALSymbol symbol) : this()
        {
            this.range = symbol.range;
        }

    }
}
