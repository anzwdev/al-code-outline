using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AnZwDev.AL.Symbols.Parsing;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppCodeunitSymbol : AppObjectWithCodeSymbol<CodeunitSymbol>
    {

        [JsonPropertyName("ImplementedInterfaces")]
        public List<string>? ImplementedInterfaces { get; set; }

        public override CodeunitSymbol CreateSymbol(string? ns)
        {
            var symbol = new CodeunitSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), AppPropertySymbol.CreatePropertySymbolsCollection(Properties))
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Variables = Variables.CreateSymbolsList<GlobalVariableDeclarationSymbol, AppGlobalVariableSymbol>(ns),
                Methods = Methods.CreateSymbolsList<MethodSymbol, AppMethodSymbol>(ns),
                ImplementedInterfaces = ALSymbolExpressionParser.ParseObjectReferenceListOrNull(ObjectKind.Interface, ImplementedInterfaces, null),
                Usings = null
            };

            return symbol;
        }


    }
}
