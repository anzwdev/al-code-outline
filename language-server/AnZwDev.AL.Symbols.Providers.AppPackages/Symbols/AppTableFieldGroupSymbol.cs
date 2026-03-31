using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppTableFieldGroupSymbol : AppSerializedSymbol<TableFieldGroupSymbol>
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("FieldNames")]
        public string[]? FieldNames { get; set; }

        public override TableFieldGroupSymbol CreateSymbol(string? ns)
        {
            return new TableFieldGroupSymbol()
            {
                Name = Name ?? String.Empty,
                FieldNames = (FieldNames != null) ? FieldNames.ToList() : new List<string>()
            };
        }

    }
}
