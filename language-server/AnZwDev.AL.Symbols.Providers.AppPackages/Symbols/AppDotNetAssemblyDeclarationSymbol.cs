using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppDotNetAssemblyDeclarationSymbol : AppSerializedSymbol<DotNetAssemblyDeclarationSymbol>
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("TypeDeclarations")]
        public AppDotNetTypeDeclarationSymbol[]? TypeDeclarations { get; set; }

        public override DotNetAssemblyDeclarationSymbol CreateSymbol(string? ns)
        {
            return new DotNetAssemblyDeclarationSymbol()
            {
                Name = Name ?? String.Empty,
                Properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties),
                TypeDeclarations = TypeDeclarations.CreateSymbolsListOrNull<DotNetTypeDeclarationSymbol, AppDotNetTypeDeclarationSymbol>(ns)
            };
        }

    }
}
