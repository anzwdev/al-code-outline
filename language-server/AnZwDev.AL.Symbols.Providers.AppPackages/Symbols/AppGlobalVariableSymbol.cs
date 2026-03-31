using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppGlobalVariableSymbol : AppSerializedSymbol<GlobalVariableDeclarationSymbol>
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Protected")]
        public bool Protected { get; set; }

        [JsonPropertyName("TypeDefinition")]
        public AppTypeDefinitionSymbol? TypeDefinition { get; set; }

        [JsonPropertyName("Attributes")]
        public AppAttributeSymbol[]? Attributes { get; set; }

        public override GlobalVariableDeclarationSymbol CreateSymbol(string? ns)
        {
            var symbol = new GlobalVariableDeclarationSymbol()
            {
                Name = Name ?? String.Empty,
                Protected = Protected,
                TypeDefinition = TypeDefinition?.CreateSymbol(ns),
                Attributes = Attributes.CreateSymbolsListOrNull<AttributeSymbol, AppAttributeSymbol>(ns)
            };

            return symbol;
        }

    }
}
