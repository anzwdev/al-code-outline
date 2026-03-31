using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppQueryColumnSymbol : AppSerializedSymbol<QueryColumnSymbol>
    {

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("SourceColumn")]
        public string? SourceColumn { get; set; }

        public override QueryColumnSymbol CreateSymbol(string? ns)
        {
            return new QueryColumnSymbol()
            {
                Id = Id,
                Name = Name ?? String.Empty,
                Properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties),
                SourceColumn = SourceColumn
            };
        }

    }
}
