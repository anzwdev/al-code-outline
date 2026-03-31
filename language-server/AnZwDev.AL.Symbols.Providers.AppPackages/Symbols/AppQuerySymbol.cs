using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppQuerySymbol : AppObjectWithCodeSymbol<QuerySymbol>
    {

        [JsonPropertyName("Elements")]
        public AppQueryDataItemSymbol[]? Elements { get; set; }

        public override QuerySymbol CreateSymbol(string? ns)
        {
            return new QuerySymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), AppPropertySymbol.CreatePropertySymbolsCollection(Properties))
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Variables = Variables.CreateSymbolsList<GlobalVariableDeclarationSymbol, AppGlobalVariableSymbol>(ns),
                Methods = Methods.CreateSymbolsList<MethodSymbol, AppMethodSymbol>(ns),
                Elements = Elements.CreateSymbolsList<QueryDataItemSymbol, AppQueryDataItemSymbol>(ns),
                Usings = null
            };
        }


    }
}
