using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal abstract class AppBaseMethodSymbol<T> :  AppSerializedSymbol<T> where T : MethodSymbol
    {

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("MethodKind")]
        public int MethodKind { get; set; }

        [JsonPropertyName("Attributes")]
        public AppAttributeSymbol[]? Attributes { get; set; }

        [JsonPropertyName("IsInternal")]
        public bool IsInternal { get; set; }

        [JsonPropertyName("IsLocal")]
        public bool IsLocal { get; set; }

        [JsonPropertyName("IsProtected")]
        public bool IsProtected { get; set; }

        [JsonPropertyName("Parameters")]
        public AppMethodParameterSymbol[]? Parameters { get; set; }

        [JsonPropertyName("ReturnTypeDefinition")]
        public AppReturnTypeDefinitionSymbol? ReturnTypeDefinition { get; set; }


    }
}
