using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppEnumValueSymbol : AppSerializedSymbol<EnumValueSymbol>
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("Ordinal")]
        public int Ordinal { get; set; }

        public override EnumValueSymbol CreateSymbol(string? ns)
        {
            return new EnumValueSymbol()
            {
                Name = Name ?? String.Empty,
                Properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties),
                Ordinal = Ordinal
            };
        }

    }
}
