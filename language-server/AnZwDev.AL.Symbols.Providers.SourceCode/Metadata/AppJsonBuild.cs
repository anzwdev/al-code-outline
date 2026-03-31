using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.Metadata
{
    internal class AppJsonBuild
    {

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("by")]
        public string? By { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("url")]
        public string? Url { get; set; }


        public ApplicationBuildInformation CreateSymbol()
        {
            return new ApplicationBuildInformation()
            {
                By = By,
                Url = Url
            };
        }

    }
}
