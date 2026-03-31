using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppDotNetPackageSymbol : AppObjectSymbol<DotNetPackageSymbol>
    {

        [JsonPropertyName("AssemblyDeclarations")]
        public AppDotNetAssemblyDeclarationSymbol[]? AssemblyDeclarations { get; set; }

        public override DotNetPackageSymbol CreateSymbol(string? ns)
        {
            return new DotNetPackageSymbol(0, new FullyQualifiedName(ns, Name ?? String.Empty), AppPropertySymbol.CreatePropertySymbolsCollection(Properties))
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                AssemblyDeclarations = AssemblyDeclarations.CreateSymbolsListOrNull<DotNetAssemblyDeclarationSymbol, AppDotNetAssemblyDeclarationSymbol>(ns),
                Usings = null
            };
        }

    }
}
