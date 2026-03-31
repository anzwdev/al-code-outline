using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppReturnTypeDefinitionSymbol : AppSerializedSymbol<MethodReturnParameterDefinitionSymbol>
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Subtype")]
        public AppSubtypeSymbol? Subtype { get; set; }

        [JsonPropertyName("OptionMembers")]
        public string[]? OptionMembers { get; set; }

        [JsonPropertyName("Temporary")]
        public bool Temporary { get; set; }

        [JsonPropertyName("ArrayDimensions")]
        public int[]? ArrayDimensions { get; set; }

        [JsonPropertyName("TypeArguments")]
        public AppTypeDefinitionSymbol[]? TypeArguments { get; set; }

        public override MethodReturnParameterDefinitionSymbol CreateSymbol(string? ns)
        {
            return new MethodReturnParameterDefinitionSymbol()
            {
                Name = String.Empty,
                Attributes = null,
                IsVar = false,
                TypeDefinition = new TypeDefinitionSymbol()
                {
                    Name = Name ?? String.Empty,
                    Subtype = Subtype?.CreateSymbol(),
                    OptionMembers = OptionMembers?.ToList(),
                    Temporary = Temporary,
                    ArrayDimensions = ArrayDimensions?.ToList(),
                    TypeArguments = TypeArguments.CreateSymbolsListOrNull<TypeDefinitionSymbol, AppTypeDefinitionSymbol>(ns)
                }
            };
        }

    }
}
