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
    internal class AppJsonSourceCodeSource
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("commit")]
        public string? Commit { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("repositoryUrl")]
        public string? RepositoryUrl { get; set; }

        public ApplicationSourceCodeLocation CreateSymbol()
        {
            return new ApplicationSourceCodeLocation()
            {
                Commit = Commit,
                RepositoryUrl = RepositoryUrl
            };
        }

    }
}
