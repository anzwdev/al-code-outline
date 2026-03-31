using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppControlAddInSymbol : AppObjectWithIdSymbol<ControlAddInSymbol>
    {

        [JsonPropertyName("PublicKeyToken")]
        public string? PublicKeyToken { get; set; }

        [JsonPropertyName("MetadataName")]
        public string? MetadataName { get; set; }

        [JsonPropertyName("Methods")]
        public AppMethodSymbol[]? Methods { get; set; }

        [JsonPropertyName("Events")]
        public AppEventSymbol[]? Events { get; set; }

        public override ControlAddInSymbol CreateSymbol(string? ns)
        {
            return new ControlAddInSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), AppPropertySymbol.CreatePropertySymbolsCollection(Properties))
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                PublicKeyToken = PublicKeyToken,
                MetadataName = MetadataName,
                Methods = Methods.CreateSymbolsListOrNull<MethodSymbol, AppMethodSymbol>(ns),
                Events = Events.CreateSymbolsListOrNull<EventSymbol, AppEventSymbol>(ns),
                Usings = null
            };
        }


    }
}
