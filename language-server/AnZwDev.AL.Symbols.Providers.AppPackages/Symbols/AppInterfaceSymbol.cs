using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppInterfaceSymbol : AppObjectWithIdSymbol<InterfaceSymbol>
    {

        [JsonPropertyName("Methods")]
        public AppMethodSymbol[]? Methods { get; set; }

        [JsonPropertyName("ExtendedInterfaces")]
        public List<string>? ExtendedInterfaces { get; set; }

        public override InterfaceSymbol CreateSymbol(string? ns)
        {
            return new InterfaceSymbol(0, new FullyQualifiedName(ns, Name ?? String.Empty), AppPropertySymbol.CreatePropertySymbolsCollection(Properties))
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Methods = Methods.CreateSymbolsList<MethodSymbol, AppMethodSymbol>(ns),
                ExtendedInterfaces = ALSymbolExpressionParser.ParseObjectReferenceListOrNull(ObjectKind.Interface, ExtendedInterfaces, null),
                Usings = null
            };
        }


    }
}
