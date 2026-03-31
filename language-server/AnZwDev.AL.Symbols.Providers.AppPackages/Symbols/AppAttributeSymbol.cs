using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppAttributeSymbol : AppSerializedSymbol<AttributeSymbol>
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Arguments")]
        public AppAttributeArgumentSymbol[]? Arguments { get; set; }

        public override AttributeSymbol CreateSymbol(string? ns)
        {
            return new AttributeSymbol()
            {
                Name = Name ?? String.Empty,
                Arguments =  AppAttributeArgumentSymbol.CreateSymbolCollection(Arguments)
            };
        }

    }
}
