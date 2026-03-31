using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppMethodParameterSymbol : AppSerializedSymbol<MethodParameterSymbol>
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("TypeDefinition")]
        public AppTypeDefinitionSymbol? TypeDefinition { get; set; }

        [JsonPropertyName("IsVar")]
        public bool? IsVar { get; set; }

        public override MethodParameterSymbol CreateSymbol(string? ns)
        {
            return new MethodParameterSymbol()
            {
                Name = Name ?? String.Empty,
                IsVar = IsVar ?? false,
                Attributes = null,
                TypeDefinition = TypeDefinition?.CreateSymbol(ns)
            };
        }

    }
}
