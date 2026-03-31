using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppTableKeySymbol : AppSerializedSymbol<TableKeySymbol>
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("FieldNames")]
        public string[]? FieldNames { get; set; }

        public override TableKeySymbol CreateSymbol(string? ns)
        {
            return new TableKeySymbol()
            {
                Name = Name ?? String.Empty,
                Properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties),
                FieldNames = (FieldNames != null) ? FieldNames.ToList() : new List<string>()
            };
        }

    }
}
