using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Metadata;
using AnZwDev.AL.Symbols.Parsing;
using AnZwDev.System.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.Metadata
{
    internal class AppJsonDependency
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("appId")]
        public string? AppId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("publisher")]
        public string? Publisher { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }


        public ApplicationDependency CreateSymbol()
        {
            return new ApplicationDependency()
            {
                Id = (!String.IsNullOrWhiteSpace(Id)) ? Id : AppId.NotNull(),
                Name = Name.NotNull(),
                Publisher = Publisher.NotNull(),
                Version = ALSymbolExpressionParser.ParseVersion(Version, 0, 0, 0, 0)
            };
        }

    }
}
